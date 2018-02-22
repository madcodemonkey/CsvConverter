﻿using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{
    internal class ClassToCsvPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassProcessors(List<PropertyMap> mapList, CsvConverterCustomAttribute oneAttribute, ICsvConverter converter)
        {
            var onePostprocessor = converter as IClassToCsvPostprocessor;
            if (onePostprocessor == null)
            {
                throw new CsvConverterAttributeException($"The {typeof(T).Name} class has specified a type converter ({oneAttribute.ConverterType.Name}) " +
                    $"that has declared itself as a {converter.ConverterType}, but the converter does " +
                    $"not implement the {nameof(IClassToCsvPostprocessor)} interface.");
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
                    AddOnePostProcessor(oneAttribute, map, onePostprocessor);
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

            if (newMap.ClassPropertyTypeConverter != null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property has specified more than one " + 
                    $"{converter.ConverterType} converter! Only one {converter.ConverterType} converter may be specified per property.");
            }


            if (classToCsvConverter.CanHandleThisInputType(newMap.PropInformation.PropertyType))
            {
                classToCsvConverter.Initialize(oneAttribute);
                newMap.ClassPropertyTypeConverter = classToCsvConverter;
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
            var onePostProcessor = converter as IClassToCsvPostprocessor;
            if (onePostProcessor == null)
            {
                throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                     $" ({oneAttribute.ConverterType.Name}) that has declared itself as a {converter.ConverterType}, but the converter does " +
                     $"not implement the {nameof(IClassToCsvPostprocessor)} interface.");
            }

            AddOnePostProcessor(oneAttribute, newMap, onePostProcessor);
       
            // Sort the post processors if there is more than one.
            if (newMap.ClassPropertyPostprocessors.Count > 0)
                newMap.ClassPropertyPostprocessors = newMap.ClassPropertyPostprocessors.OrderBy(o => o.Order).ToList();
        }

        private void AddOnePostProcessor(CsvConverterCustomAttribute oneAttribute, PropertyMap map, IClassToCsvPostprocessor postProcessor)
        {
            postProcessor.Initialize(oneAttribute);
            map.ClassPropertyPostprocessors.Add(postProcessor);
        }
    }
}