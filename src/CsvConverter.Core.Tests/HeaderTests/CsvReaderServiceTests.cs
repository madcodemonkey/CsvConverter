using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.HeaderTests
{
    [TestClass]
    public class CsvReaderServiceTests
    {
        [TestMethod]
        public void GetRecord_CanLoadHeaderRow_ValuesComputed()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Number", "PercentageBodyFat", "PercentageMuscle", "Length", "LengthArms" })
                .Returns(new List<string> { "1", "34.56789", "78.33212", "98.34222", "67.94783" })
                .Returns(new List<string> { "2", "67.89004", "79.33212", "87.38278", "68.94783" });

            var classUnderTest = new CsvReaderService<ReadHeaderTestsData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // TODO: TESTING Mapping
            //here

            // Act
            ReadHeaderTestsData1 row1 = classUnderTest.GetRecord();
            ReadHeaderTestsData1 row2 = classUnderTest.GetRecord();
            ReadHeaderTestsData1 row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(34.57, row1.PercentageBodyFat);
            Assert.AreEqual(78.3, row1.PercentageMuscle);
            Assert.AreEqual(98.342, row1.Length);
            Assert.AreEqual(67.9478, row1.LengthArms);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(67.89, row2.PercentageBodyFat);
            Assert.AreEqual(79.3, row2.PercentageMuscle);
            Assert.AreEqual(87.383, row2.Length);
            Assert.AreEqual(68.9478, row2.LengthArms);

            Assert.IsNull(row3, "There is no 3rd row!");
            rowReaderMock.VerifyAll();
        }
    }

    internal class ReadHeaderTestsData1
    {
        [CsvConverterNumber(ColumnName = "Number")]
        public int Order { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 2)]
        public double PercentageBodyFat { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1)]
        public double PercentageMuscle { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 3)]
        public double Length { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 4)]
        public double LengthArms { get; set; }
    } 

}
