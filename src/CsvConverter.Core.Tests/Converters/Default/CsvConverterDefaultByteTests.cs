using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultByteTests
    {
        [DataTestMethod]
        [DataRow("255", (byte)255)]
        [DataRow("23", (byte)23)]
        [DataRow("", (byte)0)]
        [DataRow(null, (byte)0)]
        public void GetReadData_CanConvertNonNullableBytesWithoutAnAttribute_ValuesConverted(string inputData, byte expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultByte();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            byte actual = (byte)cut.GetReadData(typeof(byte), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [DataTestMethod]
        [DataRow("255", (byte)255)]
        [DataRow("34", (byte)34)]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void GetReadData_CanConvertNullableBytesWithoutAnAttribute_ValuesConverted(string inputData, byte? expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultByte();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            byte? actual = (byte?)cut.GetReadData(typeof(byte?), inputData, "Column1", 1, 1);

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
            var cut = new CsvConverterDefaultByte();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            byte actual = (byte)cut.GetReadData(typeof(byte), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }

        [DataTestMethod]
        [DataRow((byte)1, "1", null)]
        [DataRow((byte)23, "23", null)]
        [DataRow((byte)255, "255", null)]
        [DataRow((byte)255, "255", "")]
        [DataRow((byte)255, "255", " ")]
        [DataRow((byte)255, "255", "N0")]
        [DataRow((byte)255, "255.0", "N1")]
        [DataRow((byte)255, "255.00", "N2")]
        [DataRow((byte)20, "2,000%", "P0")] // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertByte_ByteConverted(byte inputData, string expectedData, string formatData)
        {
            // Arrange
            var cut = new CsvConverterDefaultByte();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(byte), (byte)inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow((byte)1, "1", null)]
        [DataRow((byte)23, "23", null)]
        [DataRow((byte)255, "255", null)]
        [DataRow((byte)255, "255", "")]
        [DataRow((byte)255, "255", "N0")]
        [DataRow((byte)255, "255.0", "N1")]
        [DataRow((byte)255, "255.00", "N2")]
        [DataRow((byte)20, "2,000%", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void GetWriteData_CanConvertNullableBoolean_BoolConverted(byte? inputData, string expectedData, string formatData)
        {
            // Arrange
            // Arrange
            var cut = new CsvConverterDefaultByte();
            cut.StringFormat = formatData;

            // Act
            string actualData = cut.GetWriteData(typeof(byte?), inputData, "Column1", 1, 1);

            // Windows 10 (2,000%) & Windows 7 (2,000 %) format percentages slightly differently
            // So remove spaces before comparing
            if (formatData != null && formatData.StartsWith("P"))
                actualData = actualData.Replace(" ", "");
            
            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

    }
}
