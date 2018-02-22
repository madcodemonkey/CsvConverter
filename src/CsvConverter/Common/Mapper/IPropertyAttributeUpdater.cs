using System;
using System.Collections.Generic;

namespace CsvConverter.Mapper
{
    public interface IPropertyAttributeUpdater
    {
        /// <summary>Used to update the property converters on the newly created map.</summary>
        void UpdatePropertyConverter(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter);

        /// <summary>Used to update the property pre or post converters on the newly created map</summary>
        void UpdatePropertyPreOrPostConverters(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter);

        /// <summary>Used to update the Class level convertesr on the newly minted map list</summary>
        void UpdateClassConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter);
    }
}
