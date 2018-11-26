using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{

    /// <summary>Notes:  There are no StringFormat tests here because StringFormat is only used when convert Class To CSV.</summary>
    [TestClass]
    public class CsvConverterNumber_DecimalReadTests
    {
        [TestMethod]
        public void GetRecord_CanComputeDecimalPlacesOnMultipleRows_ValuesComputed()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat", "PercentageMuscle", "Length", "LengthArms" })
                .Returns(new List<string> { "1", "34.56789", "78.33212", "98.34222", "67.94783" })
                .Returns(new List<string> { "2", "67.89004", "79.33212", "87.38278", "68.94783" })
                .Returns(new List<string> { "3", "948.5334", "80.33212", "7645.322", "69.94783" });

            var classUnderTest = new CsvReaderService<CsvConverterNumberDecimalReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberDecimalReadData1 row1 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData1 row2 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData1 row3 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData1 row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(34.57m, row1.PercentageBodyFat);
            Assert.AreEqual(78.3m, row1.PercentageMuscle);
            Assert.AreEqual(98.342m, row1.Length);
            Assert.AreEqual(67.9478m, row1.LengthArms);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(67.89m, row2.PercentageBodyFat);
            Assert.AreEqual(79.3m, row2.PercentageMuscle);
            Assert.AreEqual(87.383m, row2.Length);
            Assert.AreEqual(68.9478m, row2.LengthArms);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(80.3m, row3.PercentageMuscle);
            Assert.AreEqual(948.53m, row3.PercentageBodyFat);
            Assert.AreEqual(7645.322m, row3.Length);
            Assert.AreEqual(69.9478m, row3.LengthArms);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", 1, "35.344", 35.3, "74.454", 74.5)]
        [DataRow("2", 2, "15.244", 15.2, "21.6454", 21.6)]
        public void GetRecord_WhenClassAttributeIsUsedDecimalsAreConvertedTheSameWay_ValuesComputed(
            string orderInput, int orderExpected,
            string perBodyFatInput, double perBodyFatExpected,
            string perBodyMuscelInput, double perBodyMuscleExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat", "PercentageMuscle" })
                .Returns(new List<string> { orderInput, perBodyFatInput, perBodyMuscelInput });

            var classUnderTest = new CsvReaderService<CsvConverterNumberDecimalReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberDecimalReadData2 row1 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(orderExpected, row1.Order);
            Assert.AreEqual((decimal)perBodyFatExpected, row1.PercentageBodyFat);
            Assert.AreEqual((decimal)perBodyMuscleExpected, row1.PercentageMuscle);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", 1, "35.34", 35.3, "35.34", 35.3, "74.454", 74.454)]
        [DataRow("2", 2, "-3.45", -3.4, "-3.45", -3.5, "21.6454", 21.6454)]
        [DataRow("3", 3, "3.45", 3.4, "3.45", 3.5, "21.6454", 21.6454)]
        public void GetRecord_WhenClassAttributeIsUsedYouCanOverrideIndividualProperties_ValuesComputed(
            string orderInput, int orderExpected,
            string perBodyFatInput1, double perBodyFatExpected1,
            string perBodyFatInput2, double perBodyFatExpected2,
            string perBodyMuscelInput, double perBodyMuscleExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat1", "PercentageBodyFat2", "PercentageMuscle" })
                .Returns(new List<string> { orderInput, perBodyFatInput1, perBodyFatInput2, perBodyMuscelInput });

            var classUnderTest = new CsvReaderService<CsvConverterNumberDecimalReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberDecimalReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(orderExpected, row1.Order);
            Assert.AreEqual((decimal)perBodyFatExpected1, row1.PercentageBodyFat1, "PercentageBodyFat1 MidpointRounding.ToEven");
            Assert.AreEqual((decimal)perBodyFatExpected2, row1.PercentageBodyFat2, "PercentageBodyFat2 MidpointRounding.AwayFromZero");
            Assert.AreEqual((decimal)perBodyMuscleExpected, row1.PercentageMuscle);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }


        [DataTestMethod]
        [DataRow("1", 1, "teee", 35.3, "35.34", 35.3, "74.454", 74.454)]
        [DataRow("2", 2, "case", -3.4, "-3.45", -3.5, "21.6454", 21.6454)]
        [DataRow("3", 3, "gege", 3.4, "3.45", 3.5, "21.6454", 21.6454)]
        [ExpectedException(typeof(CsvConverterException))]
        public void GetRecord_ThrowsExceptionsForBogusData_ExceptionThrown(string orderInput, int orderExpected,
            string perBodyFatInput1, double perBodyFatExpected1,
            string perBodyFatInput2, double perBodyFatExpected2,
            string perBodyMuscelInput, double perBodyMuscleExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat1", "PercentageBodyFat2", "PercentageMuscle" })
                .Returns(new List<string> { orderInput, perBodyFatInput1, perBodyFatInput2, perBodyMuscelInput });

            var classUnderTest = new CsvReaderService<CsvConverterNumberDecimalReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberDecimalReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterNumberDecimalReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should recieve an exception above for bogus input data");
        }



    }

    internal class CsvConverterNumberDecimalReadData1
    {
        public int Order { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 2)]
        public decimal PercentageBodyFat { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1)]
        public decimal PercentageMuscle { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 3)]
        public decimal Length { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 4)]
        public decimal LengthArms { get; set; }
    }

    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(decimal))]
    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(decimal?))]
    internal class CsvConverterNumberDecimalReadData2
    {
        public int Order { get; set; }

        public decimal PercentageBodyFat { get; set; }

        public decimal? PercentageMuscle { get; set; }
    }

    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(decimal))]
    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(decimal?))]
    internal class CsvConverterNumberDecimalReadData3
    {
        public int Order { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1, Mode = MidpointRounding.ToEven)]
        public decimal PercentageBodyFat1 { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1, Mode = MidpointRounding.AwayFromZero)]
        public decimal PercentageBodyFat2 { get; set; }

        [CsvConverterNumber(AllowRounding = false)]
        public decimal? PercentageMuscle { get; set; }
    }

}
