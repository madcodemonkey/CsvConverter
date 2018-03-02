using System;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.TypeConverters
{
    [TestClass]
    public class DecimalToIntCsvToClassConverterTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        private const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        [DataRow("0", 0)]
        [DataRow("1", 1)]
        [DataRow("1.0", 1)]
        [DataRow("1.1", 1)]
        [DataRow("1.2", 1)]
        [DataRow("1.3", 1)]
        [DataRow("1.4", 1)]
        [DataRow("1.5", 2)]
        [DataRow("1.6", 2)]
        [DataRow("1.7", 2)]
        [DataRow("1.8", 2)]
        [DataRow("1.9", 2)]
        [DataRow("2.0", 2)]
        [DataRow("2.1", 2)]
        [DataRow("2.2", 2)]
        [DataRow("2.3", 2)]
        [DataRow("2.4", 2)]
        [DataRow("2.5", 3)]
        [DataRow("2.6", 3)]
        [DataRow("2.7", 3)]
        [DataRow("2.8", 3)]
        [DataRow("2.9", 3)]
        [DataRow("10,230.89", 10231)]
        public void Decimal_CanConvertNumbersWithDefaultModeEqualsAwayFromZero(string inputData, int? expected)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();
         
            // Act
            var actual = classUnderTest.Convert(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        [DataRow("0", 0)]
        [DataRow("1", 1)]
        [DataRow("1.0", 1)]
        [DataRow("1.1", 1)]
        [DataRow("1.2", 1)]
        [DataRow("1.3", 1)]
        [DataRow("1.4", 1)]
        [DataRow("1.5", 2)] // ToEven rounds to the nearest EVEN number, which is two.
        [DataRow("1.6", 2)]
        [DataRow("1.7", 2)]
        [DataRow("1.8", 2)]
        [DataRow("1.9", 2)]
        [DataRow("2.0", 2)]
        [DataRow("2.1", 2)]
        [DataRow("2.2", 2)]
        [DataRow("2.3", 2)]
        [DataRow("2.4", 2)]
        [DataRow("2.5", 2)] // ToEven rounds to the nearest EVEN number, which is two.
        [DataRow("2.6", 3)]
        [DataRow("2.7", 3)]
        [DataRow("2.8", 3)]
        [DataRow("2.9", 3)]
        [DataRow("10,230.89", 10231)]
        public void Decimal_CanConvertNumbersWithDefaultModeEqualsToEven(string inputData, int? expected)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();
            var attribute = new CsvConverterMathRoundingAttribute(typeof(DecimalToIntCsvToClassConverter)) { Mode = MidpointRounding.ToEven };
            classUnderTest.Initialize(attribute);

            // Act
            var actual = classUnderTest.Convert(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        [DataRow("0", 0)]
        [DataRow("1", 1)]
        [DataRow("1.0", 1)]
        [DataRow("1.1", 1)]
        [DataRow("1.2", 1)]
        [DataRow("1.3", 1)]
        [DataRow("1.4", 1)]
        [DataRow("1.5", 1)]
        [DataRow("1.6", 1)]
        [DataRow("1.7", 1)]
        [DataRow("1.8", 1)]
        [DataRow("1.9", 1)]
        [DataRow("10,230.89", 10230)]
        public void Decimal_CanConvertNumbersWhenRoundingNotAllowed(string inputData, int? expected)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();
            var attribute = new CsvConverterMathRoundingAttribute(typeof(DecimalToIntCsvToClassConverter)) { AllowRounding = false };
            classUnderTest.Initialize(attribute);

            // Act
            var actual = classUnderTest.Convert(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("10ooo")]
        [DataRow("abc")]
        [DataRow("not number")]
        [ExpectedException(typeof(ArgumentException))]
        public void Decimal_ThrowsExceptionForNonNumbers(string inputData)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();

            // Act
            classUnderTest.Convert(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());
        }


        [DataTestMethod]
        [DataRow("", null)]
        [DataRow(null, null)]
        [DataRow("0", 0)]
        [DataRow("1", 1)]
        [DataRow("1.0", 1)]
        [DataRow("1.1", 1)]
        [DataRow("1.2", 1)]
        [DataRow("1.3", 1)]
        [DataRow("1.4", 1)]
        [DataRow("1.5", 2)]
        [DataRow("1.6", 2)]
        [DataRow("10,230.89", 10231)]
        public void NullableDecimal_CanConvertNumbers(string inputData, int? expected)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();

            // Act
            var actual = classUnderTest.Convert(typeof(decimal?), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("10ooo")]
        [DataRow("abc")]
        [DataRow("not number")]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableDecimal_ThrowsExceptionForNonNumbers(string actual)
        {
            // Arrange
            var classUnderTest = new DecimalToIntCsvToClassConverter();

            // Act
            classUnderTest.Convert(typeof(decimal?), actual, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());
        }
    }    
}
