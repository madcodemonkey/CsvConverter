using System.Collections.Generic;

namespace CsvConverter.Mapper
{
    public interface IPropertyAttributeUpdater
    {
        /// <summary>Used to update the property converters on the newly created map.</summary>
        void UpdatePropertyConverter(PropertyMap newMap);

        /// <summary>Used to update the property processors (pre or post) on the newly created map</summary>
        void UpdatePropertyProcessors(PropertyMap newMap);

        /// <summary>Used to update the Class level processors on the newly minted map list</summary>
        void UpdateClassProcessors(List<PropertyMap> mapList);
    }
}
