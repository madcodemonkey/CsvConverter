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
            var onePreConverter = converter as ICsvToClassPreConverter;
            if (onePreConverter == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(ICsvToClassPreConverter)} interface.");
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
                    AddOnePreConverter(oneAttribute, map, onePreConverter);
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


            if (csvToclassConverter.CanOutputThisType(newMap.PropInformation.PropertyType))
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
            
            AddOnePreConverter(oneAttribute, newMap, onePreConverter);
            
            // Sort the PreConverters if there is more than one.
            if (newMap.CsvToClassPreConverters.Count > 0)
                newMap.CsvToClassPreConverters = newMap.CsvToClassPreConverters.OrderBy(o => o.Order).ToList();
        }
        private void AddOnePreConverter(CsvConverterCustomAttribute oneAttribute, PropertyMap map, ICsvToClassPreConverter preConverter)
        {
            if (preConverter.CanProcessType(map.PropInformation.PropertyType))
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
        }


    }
}
