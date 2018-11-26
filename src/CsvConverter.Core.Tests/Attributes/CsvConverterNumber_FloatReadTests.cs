using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    /// <summary>Notes:  There are no StringFormat tests here because StringFormat is only used when convert Class To CSV.</summary>
    [TestClass]
    public class CsvConverterNumber_FloatReadTests
    {
        [TestMethod]
        public void GetRecord_CanComputeFloatPlacesOnMultipleRows_ValuesComputed()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat", "PercentageMuscle", "Length", "LengthArms" })
                .Returns(new List<string> { "1", "34.567", "78.332", "98.342", "67.947" })
                .Returns(new List<string> { "2", "67.890", "79.332", "87.382", "68.947" })
                .Returns(new List<string> { "3", "948.533", "80.332", "7645.322", "69.947" });

            var classUnderTest = new CsvReaderService<CsvConverterNumberFloatReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberFloatReadData1 row1 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData1 row2 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData1 row3 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData1 row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(34.57f, row1.PercentageBodyFat);
            Assert.AreEqual(78.3f, row1.PercentageMuscle);
            Assert.AreEqual(98.342f, row1.Length);
            Assert.AreEqual(67.947f, row1.LengthArms);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(67.89f, row2.PercentageBodyFat);
            Assert.AreEqual(79.3f, row2.PercentageMuscle);
            Assert.AreEqual(87.382f, row2.Length);
            Assert.AreEqual(68.947f, row2.LengthArms);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(80.3f, row3.PercentageMuscle);
            Assert.AreEqual(948.53f, row3.PercentageBodyFat);
            Assert.AreEqual(7645.322f, row3.Length);
            Assert.AreEqual(69.947f, row3.LengthArms);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", 1, "35.344", 35.3, "74.454", 74.5)]
        [DataRow("2", 2, "15.244", 15.2, "21.645", 21.6)]
        public void GetRecord_WhenClassAttributeIsUsedFloatsAreConvertedTheSameWay_ValuesComputed(
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

            var classUnderTest = new CsvReaderService<CsvConverterNumberFloatReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberFloatReadData2 row1 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(orderExpected, row1.Order);
            Assert.AreEqual((float)perBodyFatExpected, row1.PercentageBodyFat);
            Assert.AreEqual((float)perBodyMuscleExpected, row1.PercentageMuscle);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", 1, "35.34", 35.3f, "35.34", 35.3f, "74.454", 74.454f)]
        [DataRow("2", 2, "-3.45", -3.4f, "-3.45", -3.5f, "21.645", 21.645f)]
        [DataRow("3", 3, "3.45", 3.4f, "3.45", 3.5f, "21.645", 21.645f)]
        public void GetRecord_WhenClassAttributeIsUsedYouCanOverrideIndividualProperties_ValuesComputed(
            string orderInput, int orderExpected,
            string perBodyFatInput1, float perBodyFatExpected1,
            string perBodyFatInput2, float perBodyFatExpected2,
            string perBodyMuscelInput, float perBodyMuscleExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "PercentageBodyFat1", "PercentageBodyFat2", "PercentageMuscle" })
                .Returns(new List<string> { orderInput, perBodyFatInput1, perBodyFatInput2, perBodyMuscelInput });

            var classUnderTest = new CsvReaderService<CsvConverterNumberFloatReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberFloatReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(orderExpected, row1.Order);
            Assert.AreEqual(perBodyFatExpected1, row1.PercentageBodyFat1, "PercentageBodyFat1 MidpointRounding.ToEven");
            Assert.AreEqual(perBodyFatExpected2, row1.PercentageBodyFat2, "PercentageBodyFat2 MidpointRounding.AwayFromZero");
            Assert.AreEqual(perBodyMuscleExpected, row1.PercentageMuscle);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }


        [DataTestMethod]
        [DataRow("1", 1, "teee", 35.3, "35.34", 35.3, "74.454", 74.454)]
        [DataRow("2", 2, "case", -3.4, "-3.45", -3.5, "21.645", 21.6454)]
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

            var classUnderTest = new CsvReaderService<CsvConverterNumberFloatReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterNumberFloatReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterNumberFloatReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should recieve an exception above for bogus input data");
        }



    }

    internal class CsvConverterNumberFloatReadData1
    {
        public int Order { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 2)]
        public float PercentageBodyFat { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1)]
        public float PercentageMuscle { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 3)]
        public float Length { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 4)]
        public float LengthArms { get; set; }
    }

    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(float))]
    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(float?))]
    internal class CsvConverterNumberFloatReadData2
    {
        public int Order { get; set; }

        public float PercentageBodyFat { get; set; }

        public float? PercentageMuscle { get; set; }
    }

    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(float))]
    [CsvConverterNumber(NumberOfDecimalPlaces = 1, TargetPropertyType = typeof(float?))]
    internal class CsvConverterNumberFloatReadData3
    {
        public int Order { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1, Mode = MidpointRounding.ToEven)]
        public float PercentageBodyFat1 { get; set; }

        [CsvConverterNumber(NumberOfDecimalPlaces = 1, Mode = MidpointRounding.AwayFromZero)]
        public float PercentageBodyFat2 { get; set; }

        [CsvConverterNumber(AllowRounding = false)]
        public float? PercentageMuscle { get; set; }
    }

}
