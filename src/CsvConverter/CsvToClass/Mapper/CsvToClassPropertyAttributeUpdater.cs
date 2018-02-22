using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass.Mapper
{
    internal class CsvToClassPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassProcessors(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePreprocessor = converter as ICsvToClassPreprocessor;
            if (onePreprocessor == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(ICsvToClassPreprocessor)} interface.");
            }

            foreach (var map in mapList)
            {
                if (map.IgnoreWhenWriting)
                    continue;

                // All properties get this Post Processor
                // OR
                // Only certain properties get this Post Processor
                if (oneAttribute.TargetPropertyType == null || oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                {
                    AddOnePreprocessor(oneAttribute, map, onePreprocessor);
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

            if (newMap.CsvFieldTypeConverter != null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property has specified more than one csv to class converter!  " +
                    "Only one csv to class  converter may be specified per property.");
            }


            if (csvToclassConverter.CanOutputThisType(newMap.PropInformation.PropertyType))
            {
                csvToclassConverter.Initialize(oneAttribute);
                newMap.CsvFieldTypeConverter = csvToclassConverter;
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                    $" ({oneAttribute.ConverterType.Name}), but the convert does not work on a property " +
                    $"of type {newMap.PropInformation.PropertyType}!");
            }


        }

        public void UpdatePropertyProcessors(PropertyMap newMap, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePreprocessor = converter as ICsvToClassPreprocessor;
            if (onePreprocessor == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a {converter.ConverterType}, but the converter does " +
                     $"not implement the {nameof(ICsvToClassPreprocessor)} interface.");
            }
            
            AddOnePreprocessor(oneAttribute, newMap, onePreprocessor);
            
            // Sort the preprocessors if there is more than one.
            if (newMap.CsvFieldPreprocessors.Count > 0)
                newMap.CsvFieldPreprocessors = newMap.CsvFieldPreprocessors.OrderBy(o => o.Order).ToList();
        }
        private void AddOnePreprocessor(CsvConverterCustomAttribute oneAttribute, PropertyMap map, ICsvToClassPreprocessor preprocessor)
        {
            if (preprocessor.CanProcessType(map.PropInformation.PropertyType))
            {
                preprocessor.Initialize(oneAttribute);
                map.CsvFieldPreprocessors.Add(preprocessor);
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{map.PropInformation.Name}' property specified a type preprocessor " +
                    $" ({oneAttribute.ConverterType.Name}), but the preprocessor does not work on a property " +
                    $"of type {map.PropInformation.PropertyType}!");
            }
        }


    }
}
