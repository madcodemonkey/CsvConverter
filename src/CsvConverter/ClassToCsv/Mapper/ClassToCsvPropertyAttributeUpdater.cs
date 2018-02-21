using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;
using CsvConverter.Shared;

namespace CsvConverter.ClassToCsv.Mapper
{
    internal class ClassToCsvPropertyAttributeUpdater<T> : IPropertyAttributeUpdater
    {
        public void UpdateClassProcessors(List<PropertyMap> mapList)
        {
            List<ClassToCsvPostProcessorAttribute> attributeList = typeof(T).HelpFindAllClassAttributes<ClassToCsvPostProcessorAttribute>();
            if (attributeList.Count == 0)
                return;

            foreach (var oneAttribute in attributeList)
            {
                var onePostprocessor = oneAttribute.Postprocessor.HelpCreateAndCastToInterface<IClassToCsvPostprocessor>(
                    "Could not create post processor!  Make sure that you have not paired your converter with the WRONG attribute.");

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
        }

        public void UpdatePropertyConverter(PropertyMap newMap)
        {
            List<ClassToCsvTypeConverterAttribute> attributeList = newMap.PropInformation.HelpFindAllAttributes<ClassToCsvTypeConverterAttribute>();

            if (attributeList.Count == 1)
            {
                ClassToCsvTypeConverterAttribute oneAttribute = attributeList[0];
                var oneTypeConverter = oneAttribute.TypeConverter.HelpCreateAndCastToInterface<IClassToCsvTypeConverter>(
                    $"The '{newMap.PropInformation.Name}' property specified a type converter, but there is a problem!  " + 
                    "Make sure that you have not paired your converter with the WRONG attribute.");

                if (oneTypeConverter.CanHandleThisInputType(newMap.PropInformation.PropertyType))
                {
                    oneTypeConverter.Initialize(oneAttribute);
                    newMap.ClassPropertyTypeConverter = oneTypeConverter;
                }
                else
                {
                    throw new CsvConverterAttributeException($"The '{newMap.PropInformation.Name}' property specified a type converter " +
                        $" ({oneAttribute.TypeConverter.Name}), but the convert does not work on a property " +
                        $"of type {newMap.PropInformation.PropertyType}!");
                }
            }
            else if (attributeList.Count > 1)
            {
                throw new CsvConverterAttributeException($"You can only have ONE attribute that derives from {nameof(ClassToCsvTypeConverterAttribute)} " +
                    $"assigned to a given property!  There are two assigned to the class property named {newMap.PropInformation.Name}!");
            }
        }

        public void UpdatePropertyProcessors(PropertyMap newMap)
        {
            List<ClassToCsvPostProcessorAttribute> attributeList = newMap.PropInformation.HelpFindAllAttributes<ClassToCsvPostProcessorAttribute>();
            if (attributeList.Count == 0)
                return;

            foreach (var oneAttribute in attributeList)
            {
                var onePostprocessor = oneAttribute.Postprocessor.HelpCreateAndCastToInterface<IClassToCsvPostprocessor>(
                    $"The '{newMap.PropInformation.Name}' property specified a Post Processor, but there is a problem!  " +
                    "Make sure that you have not paired your post processor with the WRONG attribute.");

                AddOnePostProcessor(oneAttribute, newMap, onePostprocessor);
            }

            // Sort the post processors if there is more than one.
            if (newMap.ClassPropertyPostprocessors.Count > 0)
                newMap.ClassPropertyPostprocessors = newMap.ClassPropertyPostprocessors.OrderBy(o => o.Order).ToList();
        }

        private void AddOnePostProcessor(ClassToCsvPostProcessorAttribute oneAttribute, PropertyMap map, IClassToCsvPostprocessor postProcessor)
        {
            postProcessor.Initialize(oneAttribute);
            map.ClassPropertyPostprocessors.Add(postProcessor);
        }
    }
}
