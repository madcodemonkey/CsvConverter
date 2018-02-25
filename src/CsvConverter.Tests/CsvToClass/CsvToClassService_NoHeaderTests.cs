using System;
using System.Collections.Generic;
using CsvConverter.CsvToClass;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_NoHeaderTests
    {
        [TestMethod]
        public void CanReadCsvFileWithNoHeader()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "4", "hello" })
                .Returns(new List<string> { "6", " " });

            var classUnderTest = new CsvToClassService<CsvToClassServiceNoHeaderData>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = false;

            // Act
            CsvToClassServiceNoHeaderData row1 = classUnderTest.GetRecord();
            CsvToClassServiceNoHeaderData row2 = classUnderTest.GetRecord();
            CsvToClassServiceNoHeaderData row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(4, row1.SomeIntProperty);
            Assert.AreEqual("hello", row1.SomeStringProperty);
            Assert.AreEqual(6, row2.SomeIntProperty);
            Assert.AreEqual(" ", row2.SomeStringProperty);
            Assert.IsNull(row3, "There is no third row!");

            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void IgnoreExtraCsvColumns_IfTrueCanReadCsvWhenItContainsUnmatchedColumns()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "4", "hello", "5" })
                .Returns(new List<string> { "6", " ", "7" });

            var classUnderTest = new CsvToClassService<CsvToClassServiceNoHeaderData>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = false;
            classUnderTest.Configuration.IgnoreExtraCsvColumns = true;

            // Act
            CsvToClassServiceNoHeaderData row1 = classUnderTest.GetRecord();
            CsvToClassServiceNoHeaderData row2 = classUnderTest.GetRecord();
            CsvToClassServiceNoHeaderData row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(4, row1.SomeIntProperty);
            Assert.AreEqual("hello", row1.SomeStringProperty);
            Assert.AreEqual(6, row2.SomeIntProperty);
            Assert.AreEqual(" ", row2.SomeStringProperty);
            Assert.IsNull(row3, "There is no third row!");

            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IgnoreExtraCsvColumns_IfFalseGeneratesAnExceptionWhenItContainsUnmatchedColumns()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.RowNumber).Returns(1).Returns(2);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "4", "hello", "5" })
                .Returns(new List<string> { "6", " ", "7" });

            var classUnderTest = new CsvToClassService<CsvToClassServiceNoHeaderData>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = false;
            classUnderTest.Configuration.IgnoreExtraCsvColumns = false;

            // Act
            CsvToClassServiceNoHeaderData row = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have thrown an exception because extra columns were found.");

            rowReaderMock.VerifyAll();
        }


    }

    internal class CsvToClassServiceNoHeaderData
    {
        [CsvConverterAttribute(ColumnIndex = 1)]
        public int SomeIntProperty { get; set; }
        [CsvConverterAttribute(ColumnIndex = 2)]
        public string SomeStringProperty { get; set; }
    }
}
