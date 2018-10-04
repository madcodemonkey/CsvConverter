using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultDecimalTests
    {
        // Default is AllowRounding = false so that we don't lose percision by default!
        [DataTestMethod]
        [DataRow("12345.251", "12345.251")]
        [DataRow("12,346.25", "12346.25")]
        [DataRow("2.3%", "0.023")] 
        [DataRow("", "0")]
        [DataRow(null, "0")]
        public void GetReadData_CanConvertNonNullableDecimalsWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            decimal expected = decimal.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDecimal();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            decimal actual = (decimal)cut.GetReadData(typeof(decimal), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("58.5751234", "58.575", true, 3)]
        [DataRow("58.5750001", "58.58", true, 2)]
        [DataRow("58.5749999", "58.57", true, 2)]
        [DataRow("58.5750001", "58.6", true, 1)]
        [DataRow("58.5750001", "59", true, 0)]
        public void GetReadData_CanRoundToGivenPrecision_NumberRoundedProperly(string inputData, string expectedAsString,
            bool allowRounding, int numberOfDecimalPlaces)
        {
            // Arrange
            decimal expected = decimal.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDecimal();
            cut.AllowRounding = allowRounding;
            cut.Initialize(null, new DefaultTypeConverterFactory());
            cut.NumberOfDecimalPlaces = numberOfDecimalPlaces;

            // Act
            decimal actual = (decimal)cut.GetReadData(typeof(decimal), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("12345.254", "12345.254")]
        [DataRow("12,345.25", "12345.25")]
        [DataRow("2.3%", "0.023")]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void GetReadData_CanConvertNullableDecimalsWithoutAnAttribute_ValuesConverted(string inputData, string expectedAsString)
        {
            // Arrange
            decimal? expected = string.IsNullOrWhiteSpace(expectedAsString) ? (decimal?)null : decimal.Parse(expectedAsString);
            var cut = new CsvConverterDefaultDecimal();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            decimal? actual = (decimal?)cut.GetReadData(typeof(decimal?), inputData, "Column1", 1, 1);

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
            var cut = new CsvConverterDefaultDecimal();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            decimal actual = (decimal)cut.GetReadData(typeof(decimal), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }

        [DataTestMethod]
        [DataRow("1.0", "1.0", null)]
        [DataRow("23.0", "23.0", null)]
        [DataRow("2000.0", "2000.0", null)]
        [DataRow("2000.0", "2,000.00", "N")]
        [DataRow("2000.0", "2,000", "N0")]
        [DataRow("2000.0", "2,000.0", "N1")]
        [DataRow("2000.0", "2,000.00", "N2")]
        [DataRow(".20", "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertDecimal_DecimalConverted(string inputString, string expectedData, string formatData)
        {
            // Arrange
            decimal inputData = decimal.Parse(inputString);

            var cut = new CsvConverterDefaultDecimal();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(decimal), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");


            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1.0", "1.0", null)]
        [DataRow("23.0", "23.0", null)]
        [DataRow("2000.0", "2000.0", null)]
        [DataRow("2000.0", "2,000.00", "N")]
        [DataRow("2000.0", "2,000", "N0")]
        [DataRow("2000.0", "2,000.0", "N1")]
        [DataRow("2000.0", "2,000.00", "N2")]
        [DataRow(".20", "20%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertNullableDecimal_DecimalConverted(string inputString, string expectedData, string formatData)
        {
            // Arrange
            decimal? inputData = string.IsNullOrWhiteSpace(inputString) ? null : (decimal?)decimal.Parse(inputString);

            // Arrange
            var cut = new CsvConverterDefaultDecimal();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(decimal?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
