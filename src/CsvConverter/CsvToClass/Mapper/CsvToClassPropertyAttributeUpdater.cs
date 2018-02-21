using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass.Mapper
{
    internal class CsvToClassPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassProcessors(List<PropertyMap> mapList)
        {
            // Find preprocessors attributes on the class
            List<CsvToClassPreprocessorAttribute> attributeList = typeof(T).HelpFindAllClassAttributes<CsvToClassPreprocessorAttribute>();
            if (attributeList.Count == 0)
                return;

            foreach (var oneAttribute in attributeList)
            {
                var onePreprocessor = oneAttribute.Preprocessor.HelpCreateAndCastToInterface<ICsvToClassPreprocessor>("Could not create preprocessor!");
                foreach (var map in mapList)
                {
                    if (map.IgnoreWhenReading)
                        continue;

                    // All properties get this preprocessor
                    // OR
                    // Only certain properties get this preprocessor
                    if (oneAttribute.TargetPropertyType == null || oneAttribute.TargetPropertyType == map.PropInformation.PropertyType)
                    {
                        AddOnePreprocessor(oneAttribute, map, onePreprocessor);
                    }
                }
            }
        }

        public void UpdatePropertyConverter(PropertyMap newMap)
        {
            List<CsvToClassTypeConverterAttribute> attributeList = newMap.PropInformation.HelpFindAllAttributes<CsvToClassTypeConverterAttribute>();
            if (attributeList.Count == 1)
            {
                CsvToClassTypeConverterAttribute oneAttribute = attributeList[0];

                var oneTypeConverter = oneAttribute.TypeConverter.HelpCreateAndCastToInterface<ICsvToClassTypeConverter>(
                    $"The '{newMap.PropInformation.Name}' property specified a type converter, but there is a problem!  " +
                    "Make sure that you have not paired your converter with the WRONG attribute.");

                if (oneTypeConverter.CanOutputThisType(newMap.PropInformation.PropertyType) == false)
                {
                    throw new CsvConverterAttributeException("Convert type mismatch!  The type converter " +
                        $"{oneAttribute.TypeConverter.Name} cannot process the class property named {newMap.PropInformation.Name}, " +
                        $"which is has a type of {newMap.PropInformation.PropertyType.Name}!");
                }

                oneTypeConverter.Initialize(oneAttribute);
                newMap.CsvFieldTypeConverter = oneTypeConverter;
            }
            else if (attributeList.Count > 1)
            {
                throw new CsvConverterAttributeException($"You can only have ONE attribute that derives from {nameof(CsvToClassTypeConverterAttribute)} " +
                    $"assigned to a given property!  There are two assigned to the class property named {newMap.PropInformation.Name}!");
            }
        }

        public void UpdatePropertyProcessors(PropertyMap newMap)
        {
            List<CsvToClassPreprocessorAttribute> attributeList = newMap.PropInformation.HelpFindAllAttributes<CsvToClassPreprocessorAttribute>();
            if (attributeList.Count == 0)
                return;

            foreach (var oneAttribute in attributeList)
            {
                var onePreprocessor = oneAttribute.Preprocessor.HelpCreateAndCastToInterface<ICsvToClassPreprocessor>(
                    $"The '{newMap.PropInformation.Name}' property specified a preprocessor, but there is a problem!  " +
                    "Make sure that you have not paired your preprocessor with the WRONG attribute.");

                AddOnePreprocessor(oneAttribute, newMap, onePreprocessor);
            }

            // Sort the preprocessors if there is more than one.
            if (newMap.CsvFieldPreprocessors.Count > 0)
                newMap.CsvFieldPreprocessors = newMap.CsvFieldPreprocessors.OrderBy(o => o.Order).ToList();
        }
        private void AddOnePreprocessor(CsvToClassPreprocessorAttribute oneAttribute, PropertyMap map, ICsvToClassPreprocessor preprocessor)
        {
            if (preprocessor.CanProcessType(map.PropInformation.PropertyType))
            {
                preprocessor.Initialize(oneAttribute);
                map.CsvFieldPreprocessors.Add(preprocessor);
            }
            else
            {
                throw new CsvConverterAttributeException($"The '{map.PropInformation.Name}' property specified a type preprocessor " +
                    $" ({oneAttribute.Preprocessor.Name}), but the preprocessor does not work on a property " +
                    $"of type {map.PropInformation.PropertyType}!");
            }
        }


    }
}
