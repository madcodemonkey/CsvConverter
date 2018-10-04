using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultDoubleTests
    {
        [DataTestMethod]
        [DataRow("12345.25", "12345.25")]
        [DataRow("12,345.25", "12345.25")]
        [DataRow("2.3%", "0.023")]
        [DataRow("", "0")]
        [DataRow(null, "0")]
        public void GetReadData_CanConvertNonNullableDoublesWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            double expected = double.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDouble();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            double actual = (double)cut.GetReadData(typeof(double), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("58.5750001", "58.58", 2, true)]
        [DataRow("58.5749999", "58.57", 2, true)]
        [DataRow("58.5750001", "58.6", 1, true)]
        [DataRow("58.5750001", "59", 0, true)]
        public void GetReadData_CanRoundToGivenPrecision_NumberRoundedProperly(
            string inputData, string expectedAsString, int numberOfDecimalPlaces, 
            bool allowRounding)
        {
            // Arrange
            double expected = double.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDouble();
            cut.AllowRounding = allowRounding;
            cut.Initialize(null, new DefaultTypeConverterFactory());
            cut.NumberOfDecimalPlaces = numberOfDecimalPlaces;

            // Act
            double actual = (double)cut.GetReadData(typeof(double), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("12345.254", "12345.254")]
        [DataRow("12,345.25", "12345.25")]
        [DataRow("2.3%", "0.023")]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void GetReadData_CanConvertNullableDoublesWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            double? expected = string.IsNullOrWhiteSpace(expectedAsString) ? (double?)null : double.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDouble();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            double? actual = (double?)cut.GetReadData(typeof(double?), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("5488e4$#@#")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetReadData_CannotHandleNonNumericStrings_ThrowsException(string inputData)
        {
            // Arrange
            var cut = new CsvConverterDefaultDouble();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            double actual = (double)cut.GetReadData(typeof(double), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }



        [DataTestMethod]
        [DataRow(1.0, "1", null)]
        [DataRow(23.0, "23", null)]
        [DataRow(2000.0, "2000", null)]
        [DataRow(2000.0, "2,000.00", "N")]
        [DataRow(2000.0, "2,000", "N0")]
        [DataRow(2000.0, "2,000.0", "N1")]
        [DataRow(2000.0, "2,000.00", "N2")]
        [DataRow(.20, "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertDouble_DoubleConverted(double inputData, string expectedData, string formatData)
        {
            // Arrange

            var cut = new CsvConverterDefaultDouble();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(double), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1.0, "1", null)]
        [DataRow(23.0, "23", null)]
        [DataRow(2000.0, "2000", null)]
        [DataRow(2000.0, "2,000.00", "N")]
        [DataRow(2000.0, "2,000", "N0")]
        [DataRow(2000.0, "2,000.0", "N1")]
        [DataRow(2000.0, "2,000.00", "N2")]
        [DataRow(.20, "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertNullableDouble_DoubleConverted(double? inputData, string expectedData, string formatData)
        {
            // Arrange            
            // Arrange
            var cut = new CsvConverterDefaultDouble();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(double?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

    }
}
