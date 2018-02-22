using System;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests
{
    [TestClass]
    public class PercentCsvToClassConverterTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        const int RowNumber = 1;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCovertJunkStrings()
        {
            // Arrange
            var classUnderTest = new PercentCsvToClassConverter();
            
            // Act
            classUnderTest.Convert(typeof(decimal), "3d5%", ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            throw new Exception("Should have encountered exception above because the string is invalid!");
        }

        [DataTestMethod]
        [DataRow("35%", ".35")]
        [DataRow(".35", ".0035")]
        [DataRow("35", ".35")]
        public void CanCovertStringWithPercentageSign(string inputData, string expected)
        {
            // Arrange
            var classUnderTest = new PercentCsvToClassConverter();
            decimal expectedResult = decimal.Parse(expected);

            // Act
            var actual = classUnderTest.Convert(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber, new DefaultStringToObjectTypeConverterManager());

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }
    }    
}
