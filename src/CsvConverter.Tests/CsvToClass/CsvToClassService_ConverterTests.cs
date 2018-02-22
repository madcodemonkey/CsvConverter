using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CsvConverter.CsvToClass;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_ConverterTests
    {
        [TestMethod]
        public void CanUseConverter()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage" })
                .Returns(new List<string> { "1", "23%" })
                .Returns(new List<string> { "2", ".23" });

            var classUnderTest = new CsvToClassService<CsvServiceConverterTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceConverterTestClass row1 = classUnderTest.GetRecord();
            CsvServiceConverterTestClass row2 = classUnderTest.GetRecord();
            CsvServiceConverterTestClass row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(.23m, row1.Percentage);
            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(.0023m, row2.Percentage);
            Assert.IsNull(row3, "There is no third row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void CannotUseConverterIfTheConvertersOutputTypeDoesNoMatchThePropertyType()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Percentage" })
                .Returns(new List<string> { "1", "23%" })
                .Returns(new List<string> { "2", ".23" });

            var classUnderTest = new CsvToClassService<CsvServiceConverterMismatchTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceConverterMismatchTestClass row = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error due to mismatched property type!");
        }



        [TestMethod]
        public void ArrayPropertyUsedWhenCoverterSpecified()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Ages" })
                .Returns(new List<string> { "1", "3,4,5" })
                .Returns(new List<string> { "2", "6,7,8" });

            var classUnderTest = new CsvToClassService<CsvServiceConverterArrayTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceConverterArrayTestClass row1 = classUnderTest.GetRecord();
            CsvServiceConverterArrayTestClass row2 = classUnderTest.GetRecord();
            CsvServiceConverterArrayTestClass row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(3, row1.Ages[0]);
            Assert.AreEqual(4, row1.Ages[1]);
            Assert.AreEqual(5, row1.Ages[2]);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(6, row2.Ages[0]);
            Assert.AreEqual(7, row2.Ages[1]);
            Assert.AreEqual(8, row2.Ages[2]);

            Assert.IsNull(row3, "There is no third row!");
            rowReaderMock.VerifyAll();
        }
    }
    
    internal class CsvServiceConverterTestClass
    {
        public int Order { get; set; }

        
        [CsvConverterCustom(typeof(PercentCsvToClassConverter))]
        public decimal Percentage { get; set; }
    }

    internal class CsvServiceConverterMismatchTestClass
    {
        public int Order { get; set; }

        [CsvConverterCustom(typeof(PercentCsvToClassConverter))]
        public double Percentage { get; set; }
    }

    internal class CsvServiceConverterArrayTestClass
    {
        public int Order { get; set; }

        [CsvConverterCustom(typeof(CommaDelimitedIntArrayCsvToClassConverter))]
        public int[] Ages { get; set; }
    }

}
