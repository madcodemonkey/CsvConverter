using CsvConverter.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


// TODO: New test - User doesn't specify a converter one is NOT create so that the default converter on the service is used.
// TODO: New Test - user can have two attributes.  One with just ignore specified and the other with a converter.  It's a waste but it is legal.
// TODO: New Test - if column name, column index or altcolumnnames are specified at the class level, you get a CsvConverterAttributeException
// TODO: New Test - user specifies column name on more than one attribute and gets a CsvConverterAttributeException exception
namespace CsvConverter.Mapper
{
    /// <summary>Base object for mapping  CSV columns to class properties</summary>
    /// <typeparam name="T">Class instance type</typeparam>
    public class ColumnToPropertyMapper<T>
    {
        /// <summary>Constructor.</summary>
        /// <param name="defaultFactory">The default type converter factory, which is passed into any 
        /// attributes found during the mapping process so that they can create a default converter if necessary.</param>
        public ColumnToPropertyMapper(CsvConverterConfiguration configuration, IDefaultTypeConverterFactory defaultFactory,
            int columnIndexDefaultValue)
        {
            _columnIndexDefaultValue = columnIndexDefaultValue;
            _configuration = configuration;
            _defaultFactory = defaultFactory;
        }

        /// <summary>Maps all the properties on the T type class and then order then by column index.</summary>
        /// <param name="configuration">Configuration items</param>
        /// <param name="columnIndexDefaultValue">The default value to give a column.  It's only relevant if the 
        /// column is never found.  So, in most cases it is 9999.</param>
        /// <returns>List of property maps sorted by column index</returns>
        public List<ColumnToPropertyMap> CreateWriteMap()
        {
            var mapList = new List<ColumnToPropertyMap>();

            foreach (PropertyInfo info in _theClassType.GetProperties())
            {
                var oneMap = new ColumnToPropertyMap(info, _columnIndexDefaultValue);

                CreateAllUserSpecifiedConvertersForOneProperty(oneMap);

                mapList.Add(oneMap);
            }

            CreateAllUserSpecifiedConvertersOnTheClass(mapList);
            CreateDefaultConverterOnAnyPropertyThatDoesNotHaveAConverter(mapList);

            // Sort the columns the way the user wants them sorted or by column name
            return mapList.Where(map => ShouldMapBeAdd(map))
                .OrderBy(o => o.ColumnIndex)
                .ThenBy(o => o.ColumnName)
                .ToList();
        }

        public Dictionary<int, ColumnToPropertyMap> CreateReadMap(List<string> headerColumns)
        {
            List<ColumnToPropertyMap> columnList = CreateWriteMap();

            if (_configuration.HasHeaderRow)
            {
                // Retrieve the header row if the file has one!
                MapClassPropertiesToCsvHeaderColumnNames(columnList, headerColumns);
            }
            else
            {
                // Nope. It's an index only mapping so did the user specify a column index on every property that is not ignored?
                ValidateThatIndexesHaveBeenSpecifiedForEveryClassProperty(columnList);
            }

            // Map all the columns into a dictionary
            // This will also discard any unmapped properties (column index still equal to ColumnIndexDefaultValue)
            var result = new Dictionary<int, ColumnToPropertyMap>();

            foreach (ColumnToPropertyMap map in columnList)
            {
                if (map.ColumnIndex > 0)
                    result.Add(map.ColumnIndex, map);

            }

            return result;
        }


        /// <summary>Maps class properties to CSV columns.</summary>
        /// <param name="mapList">The map list</param>
        /// <param name="orderedHeaderColumns">A list of header fields from the CSV file in the order the appear in the CSV.  The position of the
        /// header column will be used to determine the column index.</param>
        /// <param name="configuration">Configuration information.</param>
        private void MapClassPropertiesToCsvHeaderColumnNames(List<ColumnToPropertyMap> mapList, List<string> orderedHeaderColumns)
        {

            ResetColumnIndex(mapList);

            // Map CSV columns onto existing Properties
            for (int columnIndex = 1; columnIndex <= orderedHeaderColumns.Count; columnIndex++)
            {
                string field = orderedHeaderColumns[columnIndex - 1];
                if (string.IsNullOrWhiteSpace(field))
                {
                    // The CSV file has a blank column at this index.  We can't map it to a property so create a mapping to ignore it!
                    CreateIgnoreColumnMap(mapList, columnIndex, string.Empty);
                }
                else
                {
                    // Always trim column header fields.
                    string trimmedField = field.Trim();

                    // Find the column (even if it is being ignored) and map it to a column index!
                    List<ColumnToPropertyMap> maps = SearchForColumnName(mapList, trimmedField);
                    if (maps.Count == 1)
                    {
                        maps[0].ColumnIndex = columnIndex;
                    }
                    else if (maps.Count == 0)
                    {
                        if (_configuration.IgnoreExtraCsvColumns == false)
                        {
                            throw new ArgumentException($"The CSV file contains a column named '{trimmedField}', but we were unable to match it to a " +
                                  $"property on the {_theClassType.Name} class!  You can add a property named '{trimmedField}' to the class or " +
                                  $"put a CSV Converter attribute on the property and specify a ColumnName as '{trimmedField}' or " +
                                  $"put a CSV Converter attribute on a property and specify IgnoreWhenReading = true or " +
                                  "set IgnoreExtraCsvColumns = true in configuration.");
                        }

                        // We don't have a class property for the CSV column, so we should ignore it.
                        CreateIgnoreColumnMap(mapList, columnIndex, trimmedField);
                    }
                    else if (maps.Count > 1)
                    {
                        throw new ArgumentException($"You have more than one column mapped to the column name {trimmedField} on " +
                            $"the {_theClassType.Name} class!.  Please check the CSV converter attributes on the class properties " +
                            $"for duplicate ColumnName and AltColumnNames values.  A column can " +
                            $"ONLY be mapped to a single property!");
                    }
                }
            }

            // Remove any Properties that didn't match a CSV column
            List<ColumnToPropertyMap> unmapped = mapList.Where(w => w.ColumnIndex == _columnIndexDefaultValue).ToList();
            foreach (var item in unmapped)
                mapList.Remove(item);
        }

        private void CreateIgnoreColumnMap(List<ColumnToPropertyMap> columns, int columnIndex, string columnName)
        {
            var newItem = new ColumnToPropertyMap(null, columnIndex)
            {
                ColumnName = columnName,
                ReadConverter = null,
                IgnoreWhenReading = true,
            };

            columns.Add(newItem);
        }


        /// <summary>Finds all the maps where the ColumnName matches and returns all matches. If nothing is found, it then searches the AltColumnNames
        /// property of every map and returns all matches.</summary>
        /// <param name="mapList">List of maps</param>
        /// <param name="trimmedField">Trimmed header field name.</param>
        private List<ColumnToPropertyMap> SearchForColumnName(List<ColumnToPropertyMap> mapList, string trimmedField)
        {
            List<ColumnToPropertyMap> result = mapList
                .Where(w => w.ColumnIndex == _columnIndexDefaultValue && string.Compare(w.ColumnName, trimmedField, true) == 0)
                .ToList();

            if (result.Count == 0)
            {
                foreach (var map in mapList)
                {
                    // Do we have alternative columns and has the column index already been assigned?  
                    // If so, ignore this map.
                    if (map.AltColumnNames == null || map.ColumnIndex != _columnIndexDefaultValue)
                        continue;

                    if (map.AltColumnNames.Exists(w => string.Compare(w, trimmedField, true) == 0))
                        result.Add(map);
                }
            }

            return result;
        }


        /// <summary>Resets all the column index to -1 (unused)</summary>
        private void ResetColumnIndex(List<ColumnToPropertyMap> columns)
        {
            foreach (var column in columns)
            {
                column.ColumnIndex = _columnIndexDefaultValue; // Reset
            }
        }


























        private void CreateDefaultConverterOnAnyPropertyThatDoesNotHaveAConverter(List<ColumnToPropertyMap> mapList)
        {
            foreach (ColumnToPropertyMap oneMap in mapList)
            {
                CreateDefaultConverterForOnePropertyIfNecessary(oneMap);
            }
        }

        private void CreateDefaultConverterForOnePropertyIfNecessary(ColumnToPropertyMap newMap)
        {
            // User doesn't want to use this column
            if (newMap.IgnoreWhenReading && newMap.IgnoreWhenWriting)
                return;

            // No converter specified for a type we understand
            if (newMap.ReadConverter == null && newMap.WriteConverter == null && IsTypeAllowed(newMap.PropInformation.PropertyType))
            {
                ICsvConverter converter = _defaultFactory.CreateConverter(newMap.PropInformation.PropertyType);
                newMap.ReadConverter = newMap.IgnoreWhenReading ? null : converter;
                newMap.WriteConverter = newMap.IgnoreWhenWriting ? null : converter;
            }
        }

        /// <summary>Shorthand way of getting to the class type.</summary>
        protected Type _theClassType = typeof(T);
        private int _columnIndexDefaultValue;
        private readonly CsvConverterConfiguration _configuration;

        /// <summary>Default type converter factory.</summary>
        protected IDefaultTypeConverterFactory _defaultFactory;


        /// <summary>Looks for CsvConverterAttribute on the property using PropertyInfo
        /// and then updates any relevant info on the map</summary>
        /// <param name="oneMap">The property map to examine</param>
        /// <param name="columnIndexDefaultValue">The default column index value (csv to class and class to csv use different values)</param>
        private void CreateAllUserSpecifiedConvertersForOneProperty(ColumnToPropertyMap oneMap)
        {
            List<CsvConverterBaseAttribute> attributeList = oneMap.PropInformation.HelpFindAllAttributes<CsvConverterBaseAttribute>();
            foreach (var oneAttribute in attributeList)
            {
                ICsvConverter converter = oneAttribute.CreateConverterForProperty(_theClassType, oneMap.PropInformation, _defaultFactory);

                bool isTypeConverter = CreateOneUserSpecifiedConverterForOneProperty(oneMap, converter, oneAttribute, true);

                // Only update column information for TYPE converters.
                // Pre and Post conveters should NOT contain column information!
                if (isTypeConverter)
                {
                    UpdateColumnInformation(oneMap, oneAttribute);
                }
            }
        }

        private bool CreateOneUserSpecifiedConverterForOneProperty(ColumnToPropertyMap oneMap, ICsvConverter converter, 
            CsvConverterBaseAttribute oneAttribute, bool throwExceptionIfConverterHasAlreadyBeenSpecified)
        {           
            // Possible pre or post converter.
            if (oneAttribute is CsvConverterStringAttribute stringConverter &&
                (stringConverter.IsPreConverter || stringConverter.IsPostConverter))
            {
                AddOnePropertyPreOrPostConverter(oneMap, converter, stringConverter);
                return false;
            }
            else
            {
                AddOnePropertyTypeConverter(oneMap, converter, oneAttribute.IgnoreWhenReading, 
                    oneAttribute.IgnoreWhenWriting, throwExceptionIfConverterHasAlreadyBeenSpecified);
                return true;
            }
        }

        private void AddOnePropertyPreOrPostConverter(ColumnToPropertyMap newMap, ICsvConverter converter, CsvConverterStringAttribute oneAttribute)
        {
            if (converter is ICsvConverterString stringConvert)
            {
                if (oneAttribute.IsPostConverter)
                {
                    newMap.PostConverters.Add(stringConvert);
                    if (newMap.PostConverters.Count > 1)
                    {
                        newMap.PostConverters = newMap.PostConverters.OrderBy(o => o.Order).ToList();
                    }
                }

                if (oneAttribute.IsPreConverter)
                {
                    newMap.PreConverters.Add(stringConvert);
                    if (newMap.PreConverters.Count > 1)
                    {
                        newMap.PreConverters = newMap.PreConverters.OrderBy(o => o.Order).ToList();
                    }
                }
            }
            else
            {
                string typeOfConverter = oneAttribute.IsPreConverter ? "PRE" : "POST";
                throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                            $"the {_theClassType.Name} class has specified a {typeOfConverter} converter " +
                            $"that does not implement the {nameof(ICsvConverterString)} interface!");
            }
        }

        private void UpdateColumnInformation(ColumnToPropertyMap newMap, CsvConverterBaseAttribute oneAttribute)
        {
            // Did the attribute specify a ColumnName?
            if (oneAttribute.IsColumnNameSpecified())
            {
                // We start the Column Name out as the same as the property name.
                // Has ColumnName been specified?
                if (newMap.IsDefaultColumnName())
                {
                    // 1st attribute is trying to change it.  No problem.
                    newMap.ColumnName = oneAttribute.ColumnName.Trim(); // We trim all the header columns so trim this entry too.
                }
                else
                {
                    // 2nd attribute is trying to change it!!! ERROR!
                    throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                          $"the {_theClassType.Name} class has specified ColumnName on more than one attribute.  " +
                          $"Please only specify the ColumnName on one of the attributes!");
                }
            }

            // Did the attribute specify AltColumnNames?
            if (oneAttribute.AreAltColumnNamesSpecified())
            {
                // Has the AltColumnNames specified on another attribute?
                if (newMap.AltColumnNames.Count == 0)
                    newMap.AltColumnNames = ExtractColumnNames(oneAttribute.AltColumnNames);
                else
                {
                    throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                          $"the {_theClassType.Name} class has specified AltColumnNames on more than one attribute.  " +
                          $"Please only specify the AltColumnNames on one of the attributes!");
                }
            }

            // Did the attribute specify ColumnIndex?
            if (oneAttribute.IsColumIndexSpecified())
            {
                // Has the ColumnIndex specified on another attribute?
                if (newMap.IsDefaultColumnIndex())
                {
                    if (oneAttribute.ColumnIndex < 1)
                    {
                        throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                            $"the {_theClassType.Name} class has specified a ColumnIndex less than 1 using " +
                            $"the a CSV Converter attribute!  Column indexes are ONE based.");
                    }
                    newMap.ColumnIndex = oneAttribute.ColumnIndex;
                }
                else
                {
                    throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                          $"the {_theClassType.Name} class has specified a ColumnIndex on more than one CSV converter attribute.  " +
                          $"Please only specify the ColumnIndex on one of the attributes!");
                }
            }
            else newMap.ColumnIndex = _columnIndexDefaultValue;
        }


        private void AddOnePropertyTypeConverter(ColumnToPropertyMap newMap, ICsvConverter converter, 
            bool ignoreWhenReading, bool ignoreWhenWriting, bool throwExceptionIfConverterHasAlreadyBeenSpecified)
        {
            if (ignoreWhenReading == false)
            {
                newMap.IgnoreWhenReading = false;

                if (newMap.ReadConverter == null)
                    newMap.ReadConverter = converter;
                else if (throwExceptionIfConverterHasAlreadyBeenSpecified)
                {
                    throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                               $"the {_theClassType.Name} class has specified more than one read converter.  " +
                               $"You may only have one read converter and one write converter per property.  If you do NOT " +
                               $"specify a value on the attribute for IgnoreWhenReading and IgnoreWhenWriting, " +
                               $"the same converter is used for reading and writing.");
                }
            }

            if (ignoreWhenWriting == false)
            {
                newMap.IgnoreWhenWriting = false;

                if (newMap.WriteConverter == null)
                    newMap.WriteConverter = converter;
                else if (throwExceptionIfConverterHasAlreadyBeenSpecified)
                {
                    throw new CsvConverterAttributeException($"The {newMap.PropInformation.Name} property on " +
                               $"the {_theClassType.Name} class has specified more than one write converter.  " +
                               $"You may only have one read converter and one write converter per property.  If you do NOT " +
                               $"specify a value on the attribute for IgnoreWhenReading and IgnoreWhenWriting, " +
                               $"the same converter is used for reading and writing.");
                }

            }

            // If the user has specified an attribute for reading and writing, we don't want to 
            // flip the ignore property of the other converter accidently so we check for the 
            // presence of the converter as well.
            if (ignoreWhenReading && newMap.ReadConverter == null)
                newMap.IgnoreWhenReading = true;

            if (ignoreWhenWriting && newMap.WriteConverter == null)
                newMap.IgnoreWhenWriting = true;
        }

       

        private void CreateAllUserSpecifiedConvertersOnTheClass(List<ColumnToPropertyMap> mapList)
        {
            // Find attributes on the class
            List<CsvConverterBaseAttribute> attributeList = _theClassType.HelpFindAllClassAttributes<CsvConverterBaseAttribute>();

            foreach (var oneAttribute in attributeList)
            {
                ICsvConverter converter = oneAttribute.CreateConverterForClass(_theClassType, _defaultFactory);

                foreach (var map in mapList)
                {
                    if (oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                    {
                        CreateOneUserSpecifiedConverterForOneProperty(map, converter, oneAttribute, false);
                    }
                }
            }
        }

        /// <summary>Extracts column names for the AltColumnNames property on the CsvConverterAttribute</summary>
        /// <param name="columnNames">Comma delimited column names</param>
        /// <returns></returns>
        private List<string> ExtractColumnNames(string columnNames)
        {
            var result = new List<string>();

            foreach (var columnName in columnNames.Split(','))
            {
                if (string.IsNullOrWhiteSpace(columnName))
                    continue;

                result.Add(columnName.Trim());
            }

            return result;
        }

        /// <summary>Indicates if the type is allowed.  This is usually only used
        /// when there isn't a type converter for the property.</summary>
        protected bool IsTypeAllowed(Type theType)
        {
            // Ignore properties that are classes and arrays            
            // Note: IsClass is true for string and array properties as well 
            if ((theType != typeof(string) && theType.IsClass))
                return false;

            return true;
        }

        /// <summary>Indicates if a map should be added to the map list.  This is usually used to weed 
        /// out array or class properties (unless the user has specified a converter than can handle the
        /// properties type)</summary>
        /// <param name="newMap"></param>
        /// <returns></returns>
        protected bool ShouldMapBeAdd(ColumnToPropertyMap newMap)
        {
            if (newMap.IgnoreWhenWriting)
                return false;

            if (newMap.ReadConverter != null || newMap.WriteConverter != null)
                return true;
            
            // Base it on the property type
            return IsTypeAllowed(newMap.PropInformation.PropertyType);
        }

        /// <summary>If there were no header columns, every class property that is not ignored should have a column index.
        /// This methods makes sure this condition exists and throws exceptions if it does not exist.</summary>
        private void ValidateThatIndexesHaveBeenSpecifiedForEveryClassProperty(List<ColumnToPropertyMap> mapList)
        {
            // Remove ignored properties 
            List<ColumnToPropertyMap> unmapped = mapList.Where(w => w.IgnoreWhenReading).ToList();
            foreach (var item in unmapped)
                mapList.Remove(item);

            for (int columnIndex = 1; columnIndex <= mapList.Count; columnIndex++)
            {
                int count = mapList.Count(w => w.ColumnIndex == columnIndex);
                if (count == 1)
                    continue;

                if (count == 0)
                {
                    throw new ArgumentException("Since the CSV file does not have a header column, every property that is " +
                        $"not ignored should have a ColumnIndex specified.  Please use the CSV Converter attribute on each " +
                        $"property in the {_theClassType.Name} class and specify a ColumnIndex or IgnoreWhenReading value.  " +
                        $"FYI, ColumnIndex is ONE based.");
                }

                throw new ArgumentException($"More than one property on the {_theClassType.Name} class is marked with a ColumnIndex " +
                    $"of {columnIndex}.  Only one property can be marked with a given column index!");
            }
        }
    }
}