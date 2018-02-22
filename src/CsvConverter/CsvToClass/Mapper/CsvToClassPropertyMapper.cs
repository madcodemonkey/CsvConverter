using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.CsvToClass.Mapper
{

    public class CsvToClassPropertyMapper<T> : PropertyMapperBase<T>
    {
        private const int ColumnIndexDefaultValue = -1;

        /// <summary>Maps CSV fields to class properties</summary>
        /// <param name="orderedHeaderColumns">A list of header fields from the CSV file in the order the appear in the CSV.  The position of the
        /// header column will be used to determine the column index.</param>
        /// <param name="configuration">Configuration information.</param>
        /// <returns>A dictionary of property maps where key is the column index.</returns>
        public Dictionary<int, ICsvToClassPropertyMap> Map(List<string> orderedHeaderColumns, CsvToClassConfiguration configuration)
        {
            // Creates and orders the map list by columun index and then by column name
            List<PropertyMap> orderedMapList = CreateMaps(ColumnIndexDefaultValue);

            // Do we have a header row?
            if (orderedHeaderColumns != null && orderedHeaderColumns.Count > 0)
            {
                // Yes. Header row.
                MapClassPropertiesToCsvHeaderColumnNames(orderedMapList, orderedHeaderColumns, configuration);
            }
            else
            {
                // Nope. It's an index only mapping so did the user specify a column index on every property that is not ignored?
                ValidateThatIndexesHaveBeenSpecifiedForEveryClassProperty(orderedMapList);
            }

            // Map all the columns into a dictionary
            // This will also discard any unmapped properties (column index still equal to -1)
            var result = new Dictionary<int, ICsvToClassPropertyMap>();

            foreach (PropertyMap map in orderedMapList)
            {
                if (map.ColumnIndex >= 0)
                    result.Add(map.ColumnIndex, map);
            }

            return result;
        }

        #region Protected
        protected override bool ShouldMapBeAdd(PropertyMap newMap)
        {
            // If a converter was specified, it will check the output type for mismatched
            if (newMap.CsvToClassTypeConverter != null)
                return true;

            // Base it on the property type
            return IsTypeAllowed(newMap.PropInformation.PropertyType);
        }
        #endregion // Protected

        #region Private
        private void CreateIgnoreColumnMap(List<PropertyMap> columns, int columnIndex, string columnName)
        {
            var newItem = new PropertyMap
            {
                ColumnIndex = columnIndex,
                ColumnName = columnName,
                CsvToClassTypeConverter = null,
                IgnoreWhenReading = true,
                PropInformation = null
            };

            columns.Add(newItem);
        }

        /// <summary>Maps class properties to CSV columns.</summary>
        /// <param name="mapList">The map list</param>
        /// <param name="orderedHeaderColumns">A list of header fields from the CSV file in the order the appear in the CSV.  The position of the
        /// header column will be used to determine the column index.</param>
        /// <param name="configuration">Configuration information.</param>
        private void MapClassPropertiesToCsvHeaderColumnNames(List<PropertyMap> mapList, List<string> orderedHeaderColumns, CsvToClassConfiguration configuration)
        {
            ResetColumnIndex(mapList);

            // Map CSV columns onto existing Properties
            for (int columnIndex = 0; columnIndex < orderedHeaderColumns.Count; columnIndex++)
            {
                string field = orderedHeaderColumns[columnIndex];
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
                    List<PropertyMap> maps = SearchForColumnName(mapList, trimmedField);
                    if (maps.Count == 0)
                    {
                        if (configuration.IgnoreExtraCsvColumns == false)
                        {
                            throw new ArgumentException($"The CSV file contains a column named '{trimmedField}', but we were unable to match it to a " +
                                  $"property on the {typeof(T).Name} class!  You can add a property named '{trimmedField}' to the class or " +
                                  $"put a ClassToCsv attribute on a property and specify a ColumnName as '{trimmedField}' or " +
                                  $"put a ClassToCsv attribute on a property and specify Ignore = true or " +
                                  "set IgnoreExtraCsvColumns = true in configuration.");
                        }

                        // We don't have a class property for the CSV column, so we should ignore it.
                        CreateIgnoreColumnMap(mapList, columnIndex, trimmedField);
                    }
                    else if (maps.Count > 1)
                    {
                        throw new ArgumentException($"You have more than one column mapped to the column name {trimmedField}.  Please check the " +
                            " Property names, ClassToCsv attributes ColumnName and AltColumnNames for duplicates.  A column can ONLY be mapped to a single property!");
                    }
                    else
                    {
                        maps[0].ColumnIndex = columnIndex;
                    }
                }
            }

            // Remove any Properties that didn't match a CSV column
            List<PropertyMap> unmapped = mapList.Where(w => w.ColumnIndex == -1).ToList();
            foreach (var item in unmapped)
                mapList.Remove(item);
        }
 
        /// <summary>Resets all the column index to -1 (unused)</summary>
        private void ResetColumnIndex(List<PropertyMap> columns)
        {
            foreach (var column in columns)
            {
                column.ColumnIndex = -1; // Reset
            }
        }

        /// <summary>Finds all the maps where the ColumnName matches and returns all matches. If nothing is found, it then searches the AltColumnNames
        /// property of every map and returns all matches.</summary>
        /// <param name="mapList">List of maps</param>
        /// <param name="trimmedField">Trimmed header field name.</param>
        private List<PropertyMap> SearchForColumnName(List<PropertyMap> mapList, string trimmedField)
        {
            const int UnassignedColumnMap = -1;
            List<PropertyMap> result = mapList
                .Where(w => w.ColumnIndex == UnassignedColumnMap && string.Compare(w.ColumnName, trimmedField, true) == 0)
                .ToList();

            if (result.Count == 0)
            {
                foreach (var map in mapList)
                {
                    // Do we have alternative columns and has the column index already been assigned?  
                    // If so, ignore this map.
                    if (map.AltColumnNames == null || map.ColumnIndex != UnassignedColumnMap)
                        continue;

                    if (map.AltColumnNames.Exists(w => string.Compare(w, trimmedField, true) == 0))
                        result.Add(map);
                }
            }

            return result;
        }
        
        /// <summary>If there were no header columns, every class property that is not ignored should have a column index.
        /// This methods makes sure this condition exists and throws exceptions if it does not exist.</summary>
        private void ValidateThatIndexesHaveBeenSpecifiedForEveryClassProperty(List<PropertyMap> mapList)
        {
            // Remove ignored properties 
            List<PropertyMap> unmapped = mapList.Where(w => w.IgnoreWhenReading).ToList();
            foreach (var item in unmapped)
                mapList.Remove(item);

            for (int columnIndex = 0; columnIndex < mapList.Count; columnIndex++)
            {
                int count = mapList.Count(w => w.ColumnIndex == columnIndex);
                if (count == 1)
                    continue;

                if (count == 0)
                {
                    throw new ArgumentException("Since the CSV file does not have a header column, every property that is " +
                        "not ignored should have a ColumnIndex specified.  Please use the ClassToCsv attribute on each " +
                        "property in the class and specify a ColumnIndex or Ignore.  FYI, ColumnIndex is ZERO based.");
                }

                throw new ArgumentException($"More than one property on the {typeof(T).Name} class is marked with a ColumnIndex of {columnIndex}.  " +
                    "Only one property can be marked with a given column index!");
            }
        }
        #endregion // Private
    }
}
