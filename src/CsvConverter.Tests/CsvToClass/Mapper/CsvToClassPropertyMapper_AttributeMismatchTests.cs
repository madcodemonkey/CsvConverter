using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter;
using CsvConverter.ClassToCsv;
using CsvConverter.CsvToClass;
using CsvConverter.CsvToClass.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassToCsv.Tests.ClassToCsv.Mapper
{
    /// <summary>I've found that I have paired a converter with the WRONG attribute and the system 
    /// does not find the problem unless I instantiate the other service.</summary>
    [TestClass]
    public class CsvToClassPropertyMapper_AttributeMismatchTests
    {
        public const int ColumnIndexDefaultValue = 1;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanFindConverterAttributeMismatch()
        {          
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "Month", "Age", "Name" };
            var classUnderTest = new CsvToClassPropertyMapper<CsvToClassConverterMismatch>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();


            // Assert
            Assert.Fail($"The {nameof(CommaDelimitedIntArrayCsvToClassConverter)} should NOT be used with {nameof(ClassToCsvTypeConverterAttribute)}.  " +
                $"It should be paired with {nameof(CsvToClassTypeConverterAttribute)} and the mapper should find the problem and it did NOT!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanFindPreProcessorAttributeMismatch()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "Month", "Age", "Name" };
            var classUnderTest = new CsvToClassPropertyMapper<CsvToClassPreProcessorMismatch1>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();


            // Assert
            Assert.Fail($"The {nameof(TextRemoverCsvToClassPreprocessor)} should not be used with {nameof(CsvToClassPreprocessorAttribute)}.  " +
                $"It should be paired with a custom attribute named {nameof(CsvToClassPreprocessorAttribute)} and the mapper should find the problem and it did NOT!!");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanFindPreProcessorAttributeMismatchOnClass()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "Month", "Age", "Name" };
            var classUnderTest = new CsvToClassPropertyMapper<CsvToClassPreProcessorMismatch2>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();


            // Assert
            Assert.Fail($"The {nameof(TextRemoverCsvToClassPreprocessor)} should not be used with {nameof(CsvToClassPreprocessorAttribute)}.  " +
                $"It should be paired with a custom attribute named {nameof(CsvToClassPreprocessorAttribute)} and the mapper should find the problem and it did NOT!!");
        }
    }

    internal class CsvToClassConverterMismatch
    {
        public int Month { get; set; }

        public int Age { get; set; }

        // Using WRONG attribute with this converter
        [ClassToCsvTypeConverter(typeof(CommaDelimitedIntArrayCsvToClassConverter))]
        public string Name { get; set; }
    }

    internal class CsvToClassPreProcessorMismatch1
    {
        [CsvConverter(ColumnIndex = 0)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 1)]
        public int Age { get; set; }

        // Using WRONG attribute with this pre-processor
        [CsvConverter(ColumnIndex = 2)]
        [ClassToCsvPostProcessor(typeof(TextRemoverCsvToClassPreprocessor))]
        public string Name { get; set; }
    }

    // Using WRONG attribute with this pre-processor
    [ClassToCsvPostProcessor(typeof(TextRemoverCsvToClassPreprocessor))]
    internal class CsvToClassPreProcessorMismatch2
    {
        [CsvConverter(ColumnIndex = 0)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 1)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        public string Name { get; set; }
    }

}
