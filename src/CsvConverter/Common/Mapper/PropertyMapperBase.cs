using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvConverter.ClassToCsv;
using CsvConverter.CsvToClass;
using CsvConverter.Shared;

namespace CsvConverter.Mapper
{
    public abstract class PropertyMapperBase<T>
    {
        private IPropertyAttributeUpdater _classToCsvAttributeHelper = new ClassToCsv.Mapper.ClassToCsvPropertyAttributeUpdater<T>();
        private IPropertyAttributeUpdater _csvToClassAttributeHelper = new CsvToClass.Mapper.CsvToClassPropertyAttributeUpdater<T>();
                
        /// <summary>Looks for CsvConverterAttribute on the property using PropertyInfo
        /// and then updates any relevant info on the map</summary>
        /// <param name="newMap">The property map to examine</param>
        private void FindCsvConverterAttributesOnOneProperty(PropertyMap newMap, int columnIndexDefaultValue)
        {
            CsvConverterAttribute oneAttribute = newMap.PropInformation.HelpFindAttribute<CsvConverterAttribute>();
            if (oneAttribute == null)
                return;

            // Does it have a primary column name?
            if (string.IsNullOrWhiteSpace(oneAttribute.ColumnName) == false)
                newMap.ColumnName = oneAttribute.ColumnName.Trim(); // We trim all the header columns so trim this entry too.

            // Are there any alternative column names?
            if (string.IsNullOrWhiteSpace(oneAttribute.AltColumnNames) == false)
            {
                newMap.AltColumnNames = ExtractColumnNames(oneAttribute.AltColumnNames);
            }

            // Should we ignore this column in the CSV file despite the fact that it could be mapped?
            if (oneAttribute.IgnoreWhenReading)
                newMap.IgnoreWhenReading = true;

            if (oneAttribute.IgnoreWhenWriting)
                newMap.IgnoreWhenWriting = true;

            // Was an index specified?
            if (oneAttribute.ColumnIndex != int.MaxValue)
                newMap.ColumnIndex = oneAttribute.ColumnIndex;
            else newMap.ColumnIndex = columnIndexDefaultValue;

            // Was data format specified?
            if (string.IsNullOrWhiteSpace(oneAttribute.DataFormat) == false)
                newMap.ClassPropertyDataFormat = oneAttribute.DataFormat;

        }
        

        /// <summary>Interate over the class properties and generate map class for each.  
        /// It also calls several abstract methods so that attributes can be read off the properties.</summary>
        /// <returns></returns>
        protected List<PropertyMap> CreateMaps(int columnIndexDefaultValue)
        {
            var mapList = new List<PropertyMap>();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                var newMap = new PropertyMap()
                {
                    ColumnIndex = columnIndexDefaultValue,
                    ColumnName = info.Name,
                    PropInformation = info
                };

                FindCsvConverterAttributesOnOneProperty(newMap, columnIndexDefaultValue);

                FindCustomTypeConvertersOnOneProperty(newMap);

                 mapList.Add(newMap);                    
            }

            FindConvertersOnTheClass(mapList);

            // Sort the columns the way the user wants them sorted or by column name
            return mapList.Where(map => ShouldMapBeAdd(map))
                .OrderBy(o => o.ColumnIndex)
                .ThenBy(o => o.ColumnName)
                .ToList();            
        }

        private void FindCustomTypeConvertersOnOneProperty(PropertyMap newMap)
        {
            List<CsvConverterCustomAttribute> attributeList = newMap.PropInformation.HelpFindAllAttributes<CsvConverterCustomAttribute>();
            foreach (var oneAttribute in attributeList)
            {
                var oneTypeConverter = oneAttribute.ConverterType.HelpCreateAndCastToInterface<ICsvConverter>(
                    $"The '{newMap.PropInformation.Name}' property specified a {nameof(CsvConverterCustomAttribute)}, but there is a problem!  " +
                    GetCustomConverterErrorMessage(oneAttribute));
                                
                switch (oneTypeConverter.ConverterType)
                {
                    case CsvConverterTypeEnum.ClassToCsvType:
                        _classToCsvAttributeHelper.UpdatePropertyConverter(newMap, oneAttribute, oneTypeConverter);
                        break;
                    case CsvConverterTypeEnum.ClassToCsvPost:
                        _classToCsvAttributeHelper.UpdatePropertyPreOrPostConverters(newMap, oneAttribute, oneTypeConverter);
                        break;
                    case CsvConverterTypeEnum.CsvToClassType:
                        _csvToClassAttributeHelper.UpdatePropertyConverter(newMap, oneAttribute, oneTypeConverter);
                        break;
                    case CsvConverterTypeEnum.CsvToClassPre:
                        _csvToClassAttributeHelper.UpdatePropertyPreOrPostConverters(newMap, oneAttribute, oneTypeConverter);
                        break;
                }
            }
        }

        private void FindConvertersOnTheClass(List<PropertyMap> mapList)
        {
            // Find pre-converters attributes on the class
            List<CsvConverterCustomAttribute> attributeList = typeof(T).HelpFindAllClassAttributes<CsvConverterCustomAttribute>();

            foreach (var oneAttribute in attributeList)
            {
                if (oneAttribute.TargetPropertyType == null)
                {
                    throw new ArgumentException($"A {nameof(CsvConverterCustomAttribute)} was placed on the  {typeof(T).Name} " +
                        $"class, but a {nameof(CsvConverterCustomAttribute.TargetPropertyType)} was NOT specified.");
                }

                var oneTypeConverter = oneAttribute.ConverterType.HelpCreateAndCastToInterface<ICsvConverter>(
                    $"The {typeof(T).Name} class has specified a {nameof(CsvConverterCustomAttribute)}, but there is a problem!  " +
                    GetCustomConverterErrorMessage(oneAttribute));

                switch (oneTypeConverter.ConverterType)
                {
                    case CsvConverterTypeEnum.ClassToCsvType:
                    case CsvConverterTypeEnum.ClassToCsvPost:
                        _classToCsvAttributeHelper.UpdateClassConverters(mapList, oneAttribute, oneTypeConverter);
                        break;
                    case CsvConverterTypeEnum.CsvToClassType:
                    case CsvConverterTypeEnum.CsvToClassPre:
                        _csvToClassAttributeHelper.UpdateClassConverters(mapList, oneAttribute, oneTypeConverter);
                        break;
                }
            }
        }

        private string GetCustomConverterErrorMessage(CsvConverterCustomAttribute oneAttribute)
        {
            return $"All custom converters should inherit from either {nameof(IClassToCsvTypeConverter)}, {nameof(ICsvToClassTypeConverter)}, " +
                    $"{nameof(IClassToCsvPostConverter)} or {nameof(ICsvToClassPreConverter)}. All of these interfaces inherit " +
                    $"{nameof(ICsvConverter)} so the {nameof(oneAttribute.ConverterType)} type found in {nameof(CsvConverterCustomAttribute)} " +
                    $"is not a proper converter.";
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
        protected abstract bool ShouldMapBeAdd(PropertyMap newMap);
    }
}
