using System;
using System.Collections.Generic;
using System.Linq;
using CsvConverter.CsvToClass;
using CsvConverter.CsvToClass.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Readers
{
    [TestClass]
    public class CsvToClassPropertyMapper_CoreTests
    {
        public const int ColumnIndexDefaultValue = -1;

        [TestMethod]
        public void ExtraColumnInCsvFileCanBeIgnored()
        {
            //  The BODYTYPE CSV field is extra
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };
            
            var colunns = new List<string>() { "FIRSTNAME", "LASTNAME", "AGE", "BODYTYPE" };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderExample1>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();

            // Assert
            Assert.AreEqual(4, result.Count, "There should be one entry per CSV column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "FirstName" && w.ColumnIndex == 0 && w.IgnoreWhenReading == false), "Problem with FirstName column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "LastName" && w.ColumnIndex == 1 && w.IgnoreWhenReading == false), "Problem with LastName column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "Age" && w.ColumnIndex == 2 && w.IgnoreWhenReading == false), "Problem with Age column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "BODYTYPE" && w.ColumnIndex == 3 && w.IgnoreWhenReading == true), "Problem with BODYTYPE column");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtraColumnInCsvFileWillCauseExceptionsWhenNotIgnored()
        {
            //  The BODYTYPE CSV field is extra
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = false };
            var colunns = new List<string>() { "FIRSTNAME", "LASTNAME", "AGE", "BODYTYPE" };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderExample1>();

            // Act
            classUnderTest.Map(colunns, configuation).Values.ToList(); 

            // Assert
            throw new Exception("There should have been an exception due to the unmapped CSV field!");
        }

        [TestMethod]
        public void ExtraColumnInClassCanBeIgnored()
        {
            //  The LastName property is extra
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };
            var colunns = new List<string>() { "FIRSTNAME", "AGE" };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderExample1>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();

            // Assert
            Assert.AreEqual(2, result.Count, "There should be one entry per CSV column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "FirstName" && w.ColumnIndex == 0 && w.IgnoreWhenReading == false), "Problem with FirstName column");
            Assert.IsTrue(result.Exists(w => w.ColumnName == "Age" && w.ColumnIndex == 1 && w.IgnoreWhenReading == false), "Problem with Age column");            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenHeaderColumnsAreNoSpecifiedColumnIndexIsRequired1()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderExample1>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(null, configuation).Values.ToList();

            // Assert
            throw new Exception("If you don't specify header columns, you must specify column index!");
        }

        [TestMethod]
        public void WhenHeaderColumnsAreNoSpecifiedColumnIndexIsRequired2()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderExample2>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(null, configuation).Values.ToList(); 

            // Assert
            Assert.AreEqual(3, result.Count, "There should be one entry per property!");
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnName_MappingColumnToMoreThanOnePropertyWillCauseAnExcpetion1()
        {
            // // In this case two ClassToCsv attributes both specify the same column name
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "FIRSTNAME", "LASTNAME", "AGE"  };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderMappingError1Example>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ColumnName_MappingColumnToMoreThanOnePropertyWillCauseAnExcpetion2()
        {
            // In this case a PropertyName and a ClassToCsv attribute both specify the same column name

            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "FIRSTNAME", "LASTNAME", "AGE" };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderMappingError2Example>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AltColumnNames_MappingColumnToMoreThanOnePropertyWillCauseAnExcpetion()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "FIRSTNAME", "LASTNAME", "AGE" };
            var classUnderTest = new CsvToClassPropertyMapper<HeaderReaderMappingError3Example>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();
        }


        [TestMethod]
        public void CanFindConvertAttribute()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "Month", "Age", "Name" };
            var classUnderTest = new CsvToClassPropertyMapper<ReaderAttributeTestExample>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();

            // Get data
            var columnMap = result.FirstOrDefault(w => w.ColumnName == "Month");

            // Assert
            Assert.AreEqual(3, result.Count, "There should be one entry per CSV column");
            Assert.IsNotNull(columnMap);
            Assert.IsNotNull(columnMap.CsvToClassTypeConverter);
            Assert.AreEqual(typeof(DecimalToIntCsvToClassConverter), columnMap.CsvToClassTypeConverter.GetType());
        }

        [TestMethod]
        public void CanFindPreConveterAttributes()
        {
            // Arrange
            var configuation = new CsvToClassConfiguration() { IgnoreExtraCsvColumns = true };

            var colunns = new List<string>() { "Month", "Age", "Name" };
            var classUnderTest = new CsvToClassPropertyMapper<ReaderAttributeTestExample>();

            // Act
            List<ICsvToClassPropertyMap> result = classUnderTest.Map(colunns, configuation).Values.ToList();

            // Get data
            var columnMap = result.FirstOrDefault(w => w.ColumnName == "Name");
            var firstPreConverter = columnMap?.CsvToClassPreConverters?.FirstOrDefault(w => w.Order == 1);
            var secondPreConverter = columnMap?.CsvToClassPreConverters?.FirstOrDefault(w => w.Order == 2);

            // Assert
            Assert.AreEqual(3, result.Count, "There should be one entry per CSV column");
            Assert.IsNotNull(columnMap);
            Assert.IsNotNull(columnMap.CsvToClassPreConverters, "Expecting to find PreConverter");
            Assert.AreEqual(2, columnMap.CsvToClassPreConverters.Count, "Should be two of them!");


            Assert.AreEqual(typeof(TextReplacerCsvToClassPreConverter), firstPreConverter.GetType());
            Assert.AreEqual(typeof(TrimCsvToClassPreConverter), secondPreConverter.GetType());
        }
    }

    internal class HeaderReaderExample1
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public decimal PercentageBodyFat { get; set; }
    }

    internal class HeaderReaderExample2
    {
        [CsvConverterAttribute(ColumnIndex = 0)]
        public string FirstName { get; set; }

        [CsvConverterAttribute(ColumnIndex = 1)]
        public string LastName { get; set; }

        [CsvConverterAttribute(ColumnIndex = 2)]
        public int Age { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public decimal PercentageBodyFat { get; set; }
    }

    internal class HeaderReaderMappingError1Example
    {
        [CsvConverterAttribute(ColumnName = "FirstName")]
        public string First1 { get; set; }

        [CsvConverterAttribute(ColumnName = "FirstName")]
        public string First2 { get; set; }

        public int Age { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public decimal PercentageBodyFat { get; set; }
    }

    internal class HeaderReaderMappingError2Example
    {        
        public string FirstName { get; set; }

        [CsvConverterAttribute(ColumnName = "FirstName")]
        public string First2 { get; set; }

        public int Age { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public decimal PercentageBodyFat { get; set; }
    }
    
    internal class HeaderReaderMappingError3Example
    {
        [CsvConverterAttribute(AltColumnNames = "FirstName")]
        public string First1 { get; set; }

        [CsvConverterAttribute(AltColumnNames = "FirstName")]
        public string First2 { get; set; }

        public int Age { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public decimal PercentageBodyFat { get; set; }
    }
      
    internal class ReaderAttributeTestExample
    {
        [CsvConverterCustom(typeof(DecimalToIntCsvToClassConverter))]
        public int Month { get; set; }

        public int Age { get; set; }

        [CsvConverterOldAndNewValue(typeof(TextReplacerCsvToClassPreConverter), OldValue ="#", NewValue ="", Order = 1)]
        [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), Order = 2)]
        public string Name { get; set; }
    }
}
