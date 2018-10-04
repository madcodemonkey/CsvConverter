using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters    
{
    [TestClass]
    public class CsvConverterDefaultFloatTests
    {
        [DataTestMethod]
        [DataRow(1.0f, "1", null)]
        [DataRow(23.0f, "23", null)]
        [DataRow(2000.0f, "2000", null)]
        [DataRow(2000.0f, "2,000.00", "N")]
        [DataRow(2000.0f, "2,000", "N0")]
        [DataRow(2000.0f, "2,000.0", "N1")]
        [DataRow(2000.0f, "2,000.00", "N2")]
        [DataRow(.20f, "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertFloat_FloatConverted(float inputData, string expectedData, string formatData)
        {
            // Arrange
            var cut = new CsvConverterDefaultFloat();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(float), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1.0f, "1", null)]
        [DataRow(23.0f, "23", null)]
        [DataRow(2000.0f, "2000", null)]
        [DataRow(2000.0f, "2,000.00", "N")]
        [DataRow(2000.0f, "2,000", "N0")]
        [DataRow(2000.0f, "2,000.0", "N1")]
        [DataRow(2000.0f, "2,000.00", "N2")]
        [DataRow(.20f, "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertNullableFloat_FloatConverted(float? inputData, string expectedData, string formatData)
        {
            // Arrange
            var cut = new CsvConverterDefaultFloat();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(float?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }


        [DataTestMethod]
        [DataRow("12345.25", "12345.25")]
        [DataRow("12,345.25", "12345.25")]
        [DataRow("2.3%", "0.023")]
        [DataRow("", "0")]
        [DataRow(null, "0")]
        public void GetReadData_CanConvertNonNullableFloatsWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            float expected = float.Parse(expectedAsString);
            var cut = new CsvConverterDefaultFloat();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            float actual = (float)cut.GetReadData(typeof(float), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("58.575", "58.58", 2, true)]
        [DataRow("58.574", "58.57", 2, true)]
        [DataRow("58.575", "58.6", 1, true)]
        [DataRow("58.575", "59", 0, true)]
        public void GetReadData_CanRoundToGivenPrecision_NumberRoundedProperly(
            string inputData, string expectedAsString, int numberOfDecimalPlaces, bool allowRounding)
        {
            // Arrange
            float expected = float.Parse(expectedAsString);
            var cut = new CsvConverterDefaultFloat();
            cut.AllowRounding = allowRounding;
            cut.Initialize(null, new DefaultTypeConverterFactory());
            cut.NumberOfDecimalPlaces = numberOfDecimalPlaces;

            // Act
            float actual = (float)cut.GetReadData(typeof(float), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("12345.254", "12345.254")]
        [DataRow("12,345.25", "12345.25")]
        [DataRow("2.3%", "0.023")]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void GetReadData_CanConvertNullableFloatsWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            float? expected = string.IsNullOrWhiteSpace(expectedAsString) ? (float?)null : float.Parse(expectedAsString);
            var cut = new CsvConverterDefaultFloat();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            float? actual = (float?)cut.GetReadData(typeof(float?), inputData, "Column1", 1, 1);

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
            var cut = new CsvConverterDefaultFloat();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            float actual = (float)cut.GetReadData(typeof(float), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }


    }
}
