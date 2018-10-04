using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultLongTests
    {
        [DataTestMethod]
        [DataRow(1L, "1", null)]
        [DataRow(23L, "23", null)]
        [DataRow(2000L, "2000", null)]
        [DataRow(2000L, "2,000", "N0")]
        [DataRow(2000L, "2,000.0", "N1")]
        [DataRow(2000L, "2,000.00", "N2")]
        [DataRow(20L, "2,000%", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertLong_LongConverted(long inputData, string expectedData, string formatData)
        {
            // Arrange
            var cut = new CsvConverterDefaultLong();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(long), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1L, "1", null)]
        [DataRow(23L, "23", null)]
        [DataRow(2000L, "2000", null)]
        [DataRow(2000L, "2,000", "N0")]
        [DataRow(2000L, "2,000.0", "N1")]
        [DataRow(2000L, "2,000.00", "N2")]
        [DataRow(20L, "2,000%", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertNullableLong_LongConverted(long? inputData, string expectedData, string formatData)
        {
            // Arrange
            // Arrange
            var cut = new CsvConverterDefaultLong();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(long?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow("12345", 12345L)]
        [DataRow("12,345", 12345L)]
        [DataRow("", 0L)]
        [DataRow(null, 0L)]
        public void GetReadData_CanConvertNonNullableLongsWithoutAnAttribute_ValuesConverted(string inputData, long expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultLong();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            long actual = (long)cut.GetReadData(typeof(long), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("12345", 12345L)]
        [DataRow("12,345", 12345L)]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void Convert_CanConvertNullableLongsWithoutAnAttribute_ValuesConverted(string inputData, long? expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultLong();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            long? actual = (long?)cut.GetReadData(typeof(long?), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("5488e4$#@#")]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_CannotHandleNonNumericStrings_ThrowsException(string inputData)
        {
            // Arrange
            var cut = new CsvConverterDefaultLong();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            long actual = (long)cut.GetReadData(typeof(long), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }

    }
}
