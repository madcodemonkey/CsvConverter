using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.ClassToCsv;
using CsvConverter.ClassToCsv.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Readers
{
    [TestClass]
    public class ClassToCsvPropertyMapper_CoreTests
    {
        public const int ColumnIndexDefaultValue = 9999;

        [TestMethod]
        public void CanFindDataFormat()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<DataFormatAttributeTestExample>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();

            // Get data
            var columnMap1 = result.FirstOrDefault(w => w.ColumnName == "Amount");
            var columnMap2 = result.FirstOrDefault(w => w.ColumnName == "Percentage");

            // Assert
            Assert.AreEqual(2, result.Count, "There should be two entries (one per property)");
            Assert.IsNotNull(columnMap1);
            Assert.AreEqual("C2", columnMap1.ClassPropertyDataFormat);
            Assert.IsNotNull(columnMap2);
            Assert.AreEqual("P1", columnMap2.ClassPropertyDataFormat);
        }

        [TestMethod]
        public void DefaultColumSortOrderIsAlphabetical()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<WriterAttributeTestExample>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();
                        
            // Assert
            Assert.AreEqual(3, result.Count, "There should be three entries (one per property)");
            Assert.AreEqual(result[0].ColumnName, "Age");
            Assert.AreEqual(result[1].ColumnName, "Month");
            Assert.AreEqual(result[2].ColumnName, "Name");
        }

        [TestMethod]
        public void ColumnIndexChangesSortOrderIsAlphabetical()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<WriterColumnIndexSortOrderTestExample>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();

            // Assert
            Assert.AreEqual(3, result.Count, "There should be three entries (one per property)");
            Assert.AreEqual(result[0].ColumnName, "Month");
            Assert.AreEqual(result[1].ColumnName, "Age");
            Assert.AreEqual(result[2].ColumnName, "Name");
        }
        
        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void PlacingConverterOnPropertyItCannotCovertCausesAnException()
        {
            // Arrange
            var configuation = new ClassToCsvConfiguration() { };
            var classUnderTest = new ClassToCsvPropertyMapper<CoverterTypeMismatchTestExample>();

            // Act
            List<IClassToCsvPropertyMap> result = classUnderTest.Map(configuation, ColumnIndexDefaultValue).ToList();

            // Assert
            Assert.Fail("Should have received exception for placing the TrimClassToCsvTypeConverter converter on an int property.");
        }
    }


    internal class DataFormatAttributeTestExample
    {
        [CsvConverterAttribute(DataFormat = "C2")]
        public decimal Amount { get; set; }

        [CsvConverterAttribute(DataFormat = "P1")]
        public decimal Percentage { get; set; }
    }

    internal class WriterAttributeTestExample
    {
        public int Month { get; set; }

        public int Age { get; set; }

        [CsvConverterCustom(typeof(TrimClassToCsvTypeConverter))]
        public string Name { get; set; }
    }

    internal class CoverterTypeMismatchTestExample
    {
        public int Month { get; set; }

        [CsvConverterCustom(typeof(TrimClassToCsvTypeConverter))]
        public int Age { get; set; }
    
        public string Name { get; set; }
    }

    internal class WriterColumnIndexSortOrderTestExample
    {
        [CsvConverterAttribute(ColumnIndex = 1)]
        public int Month { get; set; }

        public int Age { get; set; }

        public string Name { get; set; }
    }

}
