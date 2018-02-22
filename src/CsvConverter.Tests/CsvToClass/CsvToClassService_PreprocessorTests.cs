using System;
using System.Collections.Generic;
using CsvConverter.CsvToClass;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_PreConverterTests
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

            var classUnderTest = new CsvToClassService<CsvServiceTrimPreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceTrimPreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_TrimExtraWith_TrimCtoCsvPreConverter_OnClass()
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

            var classUnderTest = new CsvToClassService<CsvServiceTrimPreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;
 
            // Act
            CsvServiceTrimPreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPreConverterTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_TrimExtraWith_TrimCtoCsvPreConverter_OnProperty()
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

            var classUnderTest = new CsvToClassService<CsvServiceTrimPropPreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceTrimPropPreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceTrimPropPreConverterTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_CanSetFieldToNullIfEmptyOrWhitespaceWith_StringIsNullOrWhiteSpaceSetToNullCtoCsvPreConverter_OnClass()
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

            var classUnderTest = new CsvToClassService<CsvServiceWhiteSpacePreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceWhiteSpacePreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceWhiteSpacePreConverterTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_CanUseMultiplePreConvereters_OnClass()
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

            var classUnderTest = new CsvToClassService<CsvServiceMultipleOnClassPreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceMultipleOnClassPreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceMultipleOnClassPreConverterTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_CanUseMultiplePreConverters_OnProperty()
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

            var classUnderTest = new CsvToClassService<CsvServiceMultipleOnPropPreConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceMultipleOnPropPreConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreConverterTestClass row3 = classUnderTest.GetRecord();
            CsvServiceMultipleOnPropPreConverterTestClass row4 = classUnderTest.GetRecord();

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

    [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), TargetPropertyType = typeof(string))]
    internal class CsvServiceTrimPreConverterTestClass
    {
        public int Order { get; set; }
        public decimal Percentage { get; set; }
        public string Name { get; set; }

        [CsvConverterAttribute(IgnoreWhenReading = true)]
        public string IgnoreName { get; set; }
    }

    internal class CsvServiceTrimPropPreConverterTestClass
    {
        public int Order { get; set; }
        public string FirstName { get; set; }

        [CsvConverterCustom(typeof(TrimCsvToClassPreConverter))]
        public string LastName { get; set; }
    }


    [CsvConverterCustom(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter), TargetPropertyType = typeof(string))]
    internal class CsvServiceWhiteSpacePreConverterTestClass
    {
        public int Order { get; set; }
        public decimal Percentage { get; set; }
        public string Name { get; set; }
    }

    [CsvConverterCustom(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter), TargetPropertyType = typeof(string), Order = 1)]
    [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), TargetPropertyType = typeof(string), Order = 2)]
    internal class CsvServiceMultipleOnClassPreConverterTestClass
    {
        public int Order { get; set; }
        public string FirstName { get; set; }      
        public string LastName { get; set; }
    }


    internal class CsvServiceMultipleOnPropPreConverterTestClass
    {
        public int Order { get; set; }

        [CsvConverterCustom(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter), Order = 1)]
        [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), Order = 2)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
