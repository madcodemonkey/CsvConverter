using System;
using System.Collections.Generic;
using CsvConverter.CsvToClass;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_WithHeaderTests
    {
        [TestMethod]
        public void Header_CanHandleExtraSpace()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { " Order ", " Percentage", "Name " })
                .Returns(new List<string> { "1", "59.5", "John" })
                .Returns(new List<string> { "2", ".23", "Bob" });

            var classUnderTest = new CsvToClassService<CsvServiceHeaderTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceHeaderTestClass row1 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row2 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);
            Assert.AreEqual("John", row1.Name);
            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);
            Assert.AreEqual("Bob", row2.Name);
            Assert.IsNull(row3, "There is no third row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void Header_CanHandleMixedCase()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { " ORDER ", " PeRceNtaGE", "nAME " })
                .Returns(new List<string> { "1", "59.5", "John" })
                .Returns(new List<string> { "2", ".23", "Bob" });

            var classUnderTest = new CsvToClassService<CsvServiceHeaderTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceHeaderTestClass row1 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row2 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);
            Assert.AreEqual("John", row1.Name);
            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);
            Assert.AreEqual("Bob", row2.Name);
            Assert.IsNull(row3, "There is no third row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void DataFields_CanReadData1()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage", "Name" })
                .Returns(new List<string> { "1", "59.5", "John" })
                .Returns(new List<string> { "2", ".23", "Bob" })
                .Returns(new List<string> { "3", ".67", "James" });

            var classUnderTest = new CsvToClassService<CsvServiceHeaderTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceHeaderTestClass row1 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row2 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row3 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row4 = classUnderTest.GetRecord();

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
        public void DataFields_CanReadData2()
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

            var classUnderTest = new CsvToClassService<CsvServiceHeaderTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceHeaderTestClass row1 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row2 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row3 = classUnderTest.GetRecord();
            CsvServiceHeaderTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(59.5m, row1.Percentage);            
            Assert.AreEqual("  ", row1.Name);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.23m, row2.Percentage);            
            Assert.AreEqual("", row2.Name);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(.67m, row3.Percentage);
            Assert.AreEqual("James ", row3.Name);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }
    }

    internal class CsvServiceHeaderTestClass
    {
        public int Order { get; set; }
        public decimal Percentage { get; set; }
        public string Name { get; set; }
    }
}
