using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{

    public class ClassToCsvPropertyMapper<T> : PropertyMapperBase<T>
    {        
        public List<IClassToCsvPropertyMap> Map(ClassToCsvConfiguration configuration, int columnIndexDefaultValue)
        {
            // Creates and orders the map list by columun index and then by column name
            List<PropertyMap> orderedMapList = CreateMaps(columnIndexDefaultValue);

            // Convert the PropertyMap list to a List of IClassToCsvPropertyMap
            return orderedMapList.OrderBy(o => o.ColumnIndex).ThenBy(o => o.ColumnName).ToList<IClassToCsvPropertyMap>();
        }

        protected override bool ShouldMapBeAdd(PropertyMap newMap)
        {
            if (newMap.IgnoreWhenWriting)
                return false;

            // If a converter was specified, it will check the output type for mismatched
            if (newMap.CsvToClassTypeConverter != null)
                return true;

            // Base it on the property type
            return IsTypeAllowed(newMap.PropInformation.PropertyType);
        }    
    }
}
