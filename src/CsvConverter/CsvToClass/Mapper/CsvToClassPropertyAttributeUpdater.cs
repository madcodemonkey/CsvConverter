using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.CsvToClass.Mapper
{
    internal class CsvToClassPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {   
            if (converter.ConverterType == CsvConverterTypeEnum.CsvToClassPre)
                UpdateClassPreConverters(mapList, oneAttribute, converter);
            else UpdateClassTypeConverters(mapList, oneAttribute, converter);
        }

        private void UpdateClassTypeConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var oneTypeConverter = converter as ICsvToClassTypeConverter;
            if (oneTypeConverter == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(ICsvToClassTypeConverter)} interface.");
            }

            foreach (var map in mapList)
            {
                if (map.IgnoreWhenReading)
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

        private void UpdateClassPreConverters(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePreConverter = converter as ICsvToClassPreConverter;
            if (onePreConverter == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(ICsvToClassPreConverter)} interface.");
            }

            foreach (var map in mapList)
            {
                if (map.IgnoreWhenReading)
                    continue;

                // All properties get this Pre Converter
                // OR
                // Only certain properties get this Pre Converter
                if (oneAttribute.TargetPropertyType == null || oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                {
                    AddOnePreConverter(map, oneAttribute, onePreConverter);
                }
            }
        }

        public void UpdatePropertyConverter(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var csvToclassConverter = converter as ICsvToClassTypeConverter;
            if (csvToclassConverter == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a csv to class converter, but the converter does " +
                     $"not implement the {nameof(ICsvToClassTypeConverter)} interface.");
            }

            if (newMap.CsvToClassTypeConverter != null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property has specified more than one csv to class converter!  " +
                    "Only one csv to class  converter may be specified per property.");
            }

            AddOneTypeConverter(newMap, oneAttribute, csvToclassConverter);

        }

        private static void AddOneTypeConverter(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvToClassTypeConverter csvToclassConverter)
        {
            if (csvToclassConverter.CanConvert(newMap.PropInformation.PropertyType))
            {
                csvToclassConverter.Initialize(oneAttribute);
                newMap.CsvToClassTypeConverter = csvToclassConverter;
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
            var onePreConverter = converter as ICsvToClassPreConverter;
            if (onePreConverter == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a {converter.ConverterType}, but the converter does " +
                     $"not implement the {nameof(ICsvToClassPreConverter)} interface.");
            }
            
            AddOnePreConverter(newMap, oneAttribute, onePreConverter);
            
        }
        private void AddOnePreConverter(PropertyMap map, CsvConverterCustomAttribute oneAttribute, ICsvToClassPreConverter preConverter)
        {
            if (preConverter.CanConvert(map.PropInformation.PropertyType))
            {
                preConverter.Initialize(oneAttribute);
                map.CsvToClassPreConverters.Add(preConverter);
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{map.PropInformation.Name}' property specified a type PreConverter " +
                    $" ({oneAttribute.ConverterType.Name}), but the PreConverter does not work on a property " +
                    $"of type {map.PropInformation.PropertyType}!");
            }

            // Sort the PreConverters if there is more than one.
            if (map.CsvToClassPreConverters.Count > 0)
                map.CsvToClassPreConverters = map.CsvToClassPreConverters.OrderBy(o => o.Order).ToList();
        }
    }
}
