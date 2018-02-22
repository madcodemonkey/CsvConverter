using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{
    internal class ClassToCsvPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePostConverter = converter as IClassToCsvPostConverter;
            if (onePostConverter == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(IClassToCsvPostConverter)} interface.");
            }
                     
            foreach (var map in mapList)
            {
                if (map.IgnoreWhenWriting)
                    continue;

                // All properties get this Post Converter
                // OR
                // Only certain properties get this Post Converter
                if (oneAttribute.TargetPropertyType == null || oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                {
                    AddOnePostConverter(oneAttribute, map, onePostConverter);
                }
            }
        }

        public void UpdatePropertyConverter(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var classToCsvConverter = converter as IClassToCsvTypeConverter;
            if (classToCsvConverter == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a {converter.ConverterType}, but the converter does " +
                     $"not implement the {nameof(IClassToCsvTypeConverter)} interface.");
            }

            if (newMap.ClassToCsvTypeConverter != null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property has specified more than one " + 
                    $"{converter.ConverterType} converter! Only one {converter.ConverterType} converter may be specified per property.");
            }


            if (classToCsvConverter.CanHandleThisInputType(newMap.PropInformation.PropertyType))
            {
                classToCsvConverter.Initialize(oneAttribute);
                newMap.ClassToCsvTypeConverter = classToCsvConverter;
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                    $" ({oneAttribute.ConverterType.Name}), but the convert does not work on a property " +
                    $"of type {newMap.PropInformation.PropertyType}!");
            }
        }

        public void UpdatePropertyPreOrPostConverters(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePostConverter = converter as IClassToCsvPostConverter;
            if (onePostConverter == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a {converter.ConverterType}, but the converter does " +
                     $"not implement the {nameof(IClassToCsvPostConverter)} interface.");
            }

            AddOnePostConverter(oneAttribute, newMap, onePostConverter);
       
            // Sort the post converters if there is more than one.
            if (newMap.ClassToCsvPostConverters.Count > 0)
                newMap.ClassToCsvPostConverters = newMap.ClassToCsvPostConverters.OrderBy(o => o.Order).ToList();
        }

        private void AddOnePostConverter(CsvConverterCustomAttribute oneAttribute, PropertyMap map, IClassToCsvPostConverter postConverter)
        {
            postConverter.Initialize(oneAttribute);
            map.ClassToCsvPostConverters.Add(postConverter);
        }
    }
}
