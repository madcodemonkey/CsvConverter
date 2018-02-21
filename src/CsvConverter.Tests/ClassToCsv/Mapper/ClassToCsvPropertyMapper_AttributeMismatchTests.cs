using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter;
using CsvConverter.ClassToCsv;
using CsvConverter.ClassToCsv.Mapper;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassToCsv.Tests.ClassToCsv.Mapper
{
    /// <summary>I've found that I have paired a converter with the WRONG attribute and the system 
    /// does not find the problem unless I instantiate the other service.</summary>
    [TestClass]
    public class ClassToCsvPropertyMapper_AttributeMismatchTests
    {
        public const int ColumnIndexDefaultValue = 9999;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanFindConverterAttributeMismatch()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<ClassToCsvConverterMismatch>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();


            // Assert
            Assert.Fail($"The {nameof(TrimClassToCsvTypeConverter)} should not be used with {nameof(CsvToClassTypeConverterAttribute)}.  " +
                $"It should be paired with {nameof(ClassToCsvTypeConverterAttribute)} and the mapper should find the problem and it did NOT!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanFindPostProcessorAttributeMismatch()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<ClassToCsvConverterMismatch>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();


            // Assert
            Assert.Fail($"The {nameof(ReplaceTextEveryMatchPostProcessor)} should not be used with {nameof(CsvToClassPreprocessorAttribute)}.  " +
                $"It should be paired with a custom attribute named {nameof(ClassToCsvReplaceTextPostprocessorAttribute)} and the mapper should find the problem and it did NOT!!");
        }
    }

    internal class ClassToCsvConverterMismatch
    {
        public int Month { get; set; }

        public int Age { get; set; }

        [CsvToClassTypeConverter(typeof(TrimClassToCsvTypeConverter))]
        public string Name { get; set; }
    }

    internal class ClassToCsvPostProcessorMismatch
    {
        [CsvConverter(ColumnIndex = 0)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 1)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        [CsvToClassPreprocessorAttribute(typeof(ReplaceTextEveryMatchPostProcessor))]
        public string Name { get; set; }
    }
}
