using System;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests
{
    [TestClass]
    public class CommaDelimitedIntArrayCsvToClassConverterTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        const int RowNumber = 1;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCovertJunkStrings()
        {
            // Arrange
            var classUnderTest = new CommaDelimitedIntArrayCsvToClassConverter();

            // Act
            classUnderTest.Convert(typeof(int[]), "35,you", ColumName, ColumnIndex, RowNumber, null);

            // Assert
            throw new Exception("Should have encountered exception above because the string is invalid!");
        }

        [TestMethod]
        public void CanCovertStringWithOneInteger()
        {
            // Arrange
            var classUnderTest = new CommaDelimitedIntArrayCsvToClassConverter();

            // Act
            // Act
            var actual = (int[]) classUnderTest.Convert(typeof(int[]), "54", ColumName, ColumnIndex, RowNumber, null);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(54, actual[0]);
        }

        [TestMethod]
        public void CanCovertStringWithMultipleIntegers()
        {
            // Arrange
            var classUnderTest = new CommaDelimitedIntArrayCsvToClassConverter();
           
            // Act
            var actual = (int[])classUnderTest.Convert(typeof(int[]), "54,34,1342", ColumName, ColumnIndex, RowNumber, null);
                  

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(54, actual[0]);
            Assert.AreEqual(34, actual[1]);
            Assert.AreEqual(1342, actual[2]);
        }
    }
}
