using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvConverter.Shared;

namespace CsvConverter.Mapper
{
    public abstract class PropertyMapperBase<T>
    {
        /// <summary>Looks for CsvConverterAttribute on the property using PropertyInfo
        /// and then updates any relevant info on the map</summary>
        /// <param name="newMap">The property map to examine</param>
        private void AddCsvConverterAttributesToTheMap(PropertyMap newMap, int columnIndexDefaultValue)
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


        /// <summary>Used to update the property converters on the newly created map.</summary>
        protected abstract void UpdatePropertyConverter(PropertyMap newMap);

        /// <summary>Used to update the property processors (pre or post) on the newly created map</summary>
        protected abstract void UpdatePropertyProcessors(PropertyMap newMap);

        /// <summary>Used to update the Class level processors on the newly minted map list</summary>
        protected abstract void UpdateClassProcessors(List<PropertyMap> mapList);


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

                AddCsvConverterAttributesToTheMap(newMap, columnIndexDefaultValue);

                UpdatePropertyConverter(newMap);

                if (ShouldMapBeAdd(newMap))
                {
                    UpdatePropertyProcessors(newMap);
                    mapList.Add(newMap);                    
                }
            }

            // Sort the columns the way the user wants them sorted or by column name
            mapList = mapList.OrderBy(o => o.ColumnIndex).ThenBy(o => o.ColumnName).ToList();

            UpdateClassProcessors(mapList);

            return mapList;
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
