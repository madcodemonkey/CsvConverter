using System;
using System.Collections.Generic;
using CsvConverter.CsvToClass;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_PreprocessorTests
    {
        [TestMethod]
        public void DataFields_CanHandleExtraUnmappedCsvColumns()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage", "Name", "Junk" })
                .Returns(new List<string> { "1", "59.5", " John ", "3" })
                .Returns(new List<string> { "2", ".23", " Bob", "3" })
                .Returns(new List<string> { "3", ".67", "James ", "3" });

            var classUnderTest = new CsvToClassService<CsvServiceTrimPreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceTrimPreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);
            Assert.AreEqual("John", row1.Name);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);
            Assert.AreEqual("Bob", row2.Name);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(.67m, row3.Percentage);
            Assert.AreEqual("James", row3.Name);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }


        [TestMethod]
        public void DataFields_TrimExtraWith_TrimCtoCsvPreprocessor_OnClass()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage", "Name" })
                .Returns(new List<string> { "1", "59.5", " John " })
                .Returns(new List<string> { "2", ".23", " Bob" })
                .Returns(new List<string> { "3", ".67", "James " });

            var classUnderTest = new CsvToClassService<CsvServiceTrimPreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;
 
            // Act
            CsvServiceTrimPreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);
            Assert.AreEqual("John", row1.Name);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);
            Assert.AreEqual("Bob", row2.Name);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(.67m, row3.Percentage);
            Assert.AreEqual("James", row3.Name);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void DataFields_TrimExtraWith_TrimCtoCsvPreprocessor_OnProperty()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "FirstName", "LastName" })
                .Returns(new List<string> { "1", "John ", "Adams " })
                .Returns(new List<string> { "2", " Bob", " Hope" })
                .Returns(new List<string> { "3", " Ralph ", " Thomponson " });

            var classUnderTest = new CsvToClassService<CsvServiceTrimPropPreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceTrimPropPreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual("John ", row1.FirstName);
            Assert.AreEqual("Adams", row1.LastName);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(" Bob", row2.FirstName);
            Assert.AreEqual("Hope", row2.LastName);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(" Ralph ", row3.FirstName);
            Assert.AreEqual("Thomponson", row3.LastName);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }
        
        [TestMethod]
        public void DataFields_CanSetFieldToNullIfEmptyOrWhitespaceWith_StringIsNullOrWhiteSpaceSetToNullCtoCsvPreprocessor_OnClass()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage", "Name" })
                .Returns(new List<string> { "1", "59.5", "  " })
                .Returns(new List<string> { "2", ".23", "" })
                .Returns(new List<string> { "3", ".67", "James " });

            var classUnderTest = new CsvToClassService<CsvServiceWhiteSpacePreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceWhiteSpacePreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);
            Assert.IsNull(row1.Name);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);
            Assert.IsNull(row2.Name);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(.67m, row3.Percentage);
            Assert.AreEqual("James ", row3.Name);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void DataFields_CanUseMultipleProcessors_OnClass()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "FirstName", "LastName" })
                .Returns(new List<string> { "1", "  ", "Adams " })
                .Returns(new List<string> { "2", " Bob", "  " })
                .Returns(new List<string> { "3", " Ralph ", " Thomponson " });

            var classUnderTest = new CsvToClassService<CsvServiceMultipleOnClassPreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceMultipleOnClassPreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.IsNull(row1.FirstName);
            Assert.AreEqual("Adams", row1.LastName);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Bob", row2.FirstName);
            Assert.IsNull(row2.LastName);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("Ralph", row3.FirstName);
            Assert.AreEqual("Thomponson", row3.LastName);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void DataFields_CanUseMultipleProcessors_OnProperty()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "FirstName", "LastName" })
                .Returns(new List<string> { "1", "  ", "Adams " })
                .Returns(new List<string> { "2", " Bob", "  " })
                .Returns(new List<string> { "3", " Ralph ", " Thomponson " });

            var classUnderTest = new CsvToClassService<CsvServiceMultipleOnPropPreprocessorTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceMultipleOnPropPreprocessorTestClass row1 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreprocessorTestClass row2 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreprocessorTestClass row3 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreprocessorTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.IsNull(row1.FirstName);
            Assert.AreEqual("Adams ", row1.LastName);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Bob", row2.FirstName);
            Assert.AreEqual("  ", row2.LastName);
       
            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("Ralph", row3.FirstName);
            Assert.AreEqual(" Thomponson ", row3.LastName);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }


    }

    [CsvToClassPreprocessor(typeof(TrimCsvToClassPreprocessor), TargetPropertyType = typeof(string))]
    internal class CsvServiceTrimPreprocessorTestClass
    {
        public int Order { get; set; }
        public decimal Percentage { get; set; }
        public string Name { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public string IgnoreName { get; set; }
    }

    internal class CsvServiceTrimPropPreprocessorTestClass
    {
        public int Order { get; set; }
        public string FirstName { get; set; }

        [CsvToClassPreprocessor(typeof(TrimCsvToClassPreprocessor))]
        public string LastName { get; set; }
    }


    [CsvToClassPreprocessor(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor), TargetPropertyType = typeof(string))]
    internal class CsvServiceWhiteSpacePreprocessorTestClass
    {
        public int Order { get; set; }
        public decimal Percentage { get; set; }
        public string Name { get; set; }
    }

    [CsvToClassPreprocessor(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor), TargetPropertyType = typeof(string), Order = 1)]
    [CsvToClassPreprocessor(typeof(TrimCsvToClassPreprocessor), TargetPropertyType = typeof(string), Order = 2)]
    internal class CsvServiceMultipleOnClassPreprocessorTestClass
    {
        public int Order { get; set; }
        public string FirstName { get; set; }      
        public string LastName { get; set; }
    }


    internal class CsvServiceMultipleOnPropPreprocessorTestClass
    {
        public int Order { get; set; }

        [CsvToClassPreprocessor(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor), Order = 1)]
        [CsvToClassPreprocessor(typeof(TrimCsvToClassPreprocessor), Order = 2)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
