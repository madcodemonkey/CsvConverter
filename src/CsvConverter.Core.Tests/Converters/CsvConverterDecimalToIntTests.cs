using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDecimalToIntTests
    {
        // Default is AllowRounding = false so that we don't lose percision by default!
        [DataTestMethod]
        [DataRow("12345.251", 12345)]
        [DataRow("12,345.65", 12346)]
        [DataRow("2.3%", 0)] 
        [DataRow("", 0)]
        [DataRow(null, 0)]
        public void GetReadData_CanConvertNonNullableDecimalsWithoutAnAttribute_ValuesConverted(string inputData, int? expected)
        {
            // Arrange
            var attribute = new CsvConverterNumberAttribute();
            var cut = new CsvConverterDecimalToInt();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());
                       
            // Act
            int actual = (int)cut.GetReadData(typeof(int), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("58.12", 58, true, 3)] // decimal places are IGNORED during reading!
        [DataRow("58.50", 59, true, 3)] // decimal places are IGNORED during reading!
        [DataRow("58.51", 59, true, 2)] // decimal places are IGNORED during reading!
        [DataRow("58.88", 59, true, 2)] // decimal places are IGNORED during reading!
        public void GetReadData_CanRoundToGivenPrecision_NumberRoundedProperly(string inputData, int? expected,
            bool allowRounding, int numberOfDecimalPlaces)
        {
            // Arrange
            var attribute = new CsvConverterNumberAttribute() { AllowRounding = allowRounding, NumberOfDecimalPlaces = numberOfDecimalPlaces };
            var cut = new CsvConverterDecimalToInt();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            int actual = (int)cut.GetReadData(typeof(int), inputData, "Column1", 1, 1);

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
            var attribute = new CsvConverterNumberAttribute();
            var cut = new CsvConverterDecimalToInt();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            decimal actual = (decimal)cut.GetReadData(typeof(decimal), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }

        [DataTestMethod]
        [DataRow(1, "1", null)]
        [DataRow(23, "23", null)]
        [DataRow(2000, "2000", null)]
        [DataRow(2000, "2,000.00", "N")]
        [DataRow(2000, "2,000", "N0")]
        [DataRow(2000, "2,000.0", "N1")]
        [DataRow(2000, "2,000.00", "N2")]
        [DataRow(2, "200%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertIntToDecimal_IntConverted(int inputData, string expectedData, string formatData)
        {
            // Arrange
            var attribute = new CsvConverterNumberAttribute() { StringFormat = formatData };
            var cut = new CsvConverterDecimalToInt();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualData = cut.GetWriteData(typeof(int), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");


            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1, "1", null)]
        [DataRow(23, "23", null)]
        [DataRow(2000, "2000", null)]
        [DataRow(2000, "2,000.00", "N")]
        [DataRow(2000, "2,000", "N0")]
        [DataRow(2000, "2,000.0", "N1")]
        [DataRow(2000, "2,000.00", "N2")]
        [DataRow(2, "200%", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void GetWriteData_CanConvertNullableDecimal_DecimalConverted(int? inputData, string expectedData, string formatData)
        {
            // Arrange
            var attribute = new CsvConverterNumberAttribute() { StringFormat = formatData };
            var cut = new CsvConverterDecimalToInt();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualData = cut.GetWriteData(typeof(int?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
