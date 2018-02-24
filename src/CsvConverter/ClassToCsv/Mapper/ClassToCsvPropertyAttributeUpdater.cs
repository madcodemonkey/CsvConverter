using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{
    internal class ClassToCsvPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            if (converter.ConverterType == CsvConverterTypeEnum.ClassToCsvPost)
                UpdateClassPostConverters(mapList, oneAttribute, converter);
            else UpdateClassTypeConverters(mapList, oneAttribute, converter);     
        }

        private void UpdateClassTypeConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var oneTypeConverter = converter as IClassToCsvTypeConverter;
            if (oneTypeConverter == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(IClassToCsvTypeConverter)} interface.");
            }

            foreach (var map in mapList)
            {
                if (map.IgnoreWhenWriting)
                    continue;

                // Property level TYPE converters override class level TYPE converters since you can ONLY have one
                // we assume the more specific property attribute is the best fit.
                if (map.ClassToCsvTypeConverter != null)
                    continue;

                // All properties get this Type Converter
                // OR
                // Only certain properties get this Type Converter
                if (oneAttribute.TargetPropertyType == null || oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                {
                    AddOneTypeConverter(map, oneAttribute, oneTypeConverter);
                }
            }
        }

        private void UpdateClassPostConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
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
                    AddOnePostConverter(map, oneAttribute, onePostConverter);
                }
            }
        }

        public void UpdatePropertyConverter(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var oneTypeConverter = converter as IClassToCsvTypeConverter;
            if (oneTypeConverter == null)
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

            AddOneTypeConverter(newMap, oneAttribute, oneTypeConverter);
        }

        private void AddOneTypeConverter(PropertyMap map, CsvConverterCustomAttribute oneAttribute, IClassToCsvTypeConverter oneTypeConverter)
        {
            if (oneTypeConverter.CanConvert(map.PropInformation.PropertyType))
            {
                oneTypeConverter.Initialize(oneAttribute);
                map.ClassToCsvTypeConverter = oneTypeConverter;
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{map.PropInformation.Name}' property specified a type converter " +
                    $" ({oneAttribute.ConverterType.Name}), but the convert does not work on a property " +
                    $"of type {map.PropInformation.PropertyType}!");
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

            AddOnePostConverter(newMap, oneAttribute, onePostConverter);
       
        }

        private void AddOnePostConverter(PropertyMap map, CsvConverterCustomAttribute oneAttribute, IClassToCsvPostConverter postConverter)
        {
            postConverter.Initialize(oneAttribute);
            map.ClassToCsvPostConverters.Add(postConverter);

            // Sort the post converters if there is more than one.
            if (map.ClassToCsvPostConverters.Count > 0)
                map.ClassToCsvPostConverters = map.ClassToCsvPostConverters.OrderBy(o => o.Order).ToList();
        }
    }
}
