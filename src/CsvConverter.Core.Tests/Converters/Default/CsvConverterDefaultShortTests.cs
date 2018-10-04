using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultShortTests
    {
        [DataTestMethod]
        [DataRow("12345", (short)12345)]
        [DataRow("12,345", (short)12345)]
        [DataRow("", (short)0)]
        [DataRow(null, (short)0)]
        public void GetReadData_CanConvertNonNullableShortsWithoutAnAttribute_ValuesConverted(string inputData, short expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultShort();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            short actual = (short)cut.GetReadData(typeof(short), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("12345", (short)12345)]
        [DataRow("12,345", (short)12345)]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void GetReadData_CanConvertNullableShortsWithoutAnAttribute_ValuesConverted(string inputData, short? expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultShort();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            short? actual = (short?)cut.GetReadData(typeof(short?), inputData, "Column1", 1, 1);

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
            var cut = new CsvConverterDefaultShort();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            short actual = (short)cut.GetReadData(typeof(short), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }


        [DataTestMethod]
        [DataRow((short)1, "1", null)]
        [DataRow((short)23, "23", null)]
        [DataRow((short)2000, "2000", null)]
        [DataRow((short)2000, "2,000", "N0")]
        [DataRow((short)2000, "2,000.0", "N1")]
        [DataRow((short)2000, "2,000.00", "N2")]
        [DataRow((short)20, "2,000%", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertInt_IntConverted(short inputData, string expectedData, string formatData)
        {
            // Arrange
            var cut = new CsvConverterDefaultShort();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(short), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow((short)1, "1", null)]
        [DataRow((short)23, "23", null)]
        [DataRow((short)2000, "2000", null)]
        [DataRow((short)2000, "2,000", "N0")]
        [DataRow((short)2000, "2,000.0", "N1")]
        [DataRow((short)2000, "2,000.00", "N2")]
        [DataRow((short) 20, "2,000%", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertNullableInt_IntConverted(short? inputData, string expectedData, string formatData)
        {
            // Arrange
            // Arrange
            var cut = new CsvConverterDefaultShort();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(short?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }

}
