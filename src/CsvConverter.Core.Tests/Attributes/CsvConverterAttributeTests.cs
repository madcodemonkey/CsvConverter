using System;
using System.Collections.Generic;
using System.Text;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterAttributeTests
    {
        [TestMethod]
        public void YouCanSpecifyBothAnIgnoreReadAndWriteOnAnObjectAndNotGetAnException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Tall" })
                .Returns(new List<string> { "1", "true", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData1>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterAttributeData1 row1 = classUnderTest.GetRecord();
            CsvConverterAttributeData1 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
         
            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }
    }

    public class CsvConverterAttributeData2
    {
        public int Age { get; set; }
    }

    public class CsvConverterAttributeData1
    {
        public int Order { get; set; }
        public bool Tall { get; set; }

        [CsvConverter(IgnoreWhenReading = true, IgnoreWhenWriting = true)]
        public CsvConverterAttributeData2 Other1 { get; set; }

        // You don't need the attribute!
        public CsvConverterAttributeData2 Other2 { get; set; }
    }


}
