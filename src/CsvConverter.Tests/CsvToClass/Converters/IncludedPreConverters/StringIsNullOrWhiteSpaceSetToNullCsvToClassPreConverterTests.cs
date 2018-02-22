using System;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests
{
    [TestClass]
    public class StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverterTests
    {
        private const string ColumnName = "SomeColumn";
        private const int ColumnIndex = 0;
        private const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("Michael", "Michael")]
        [DataRow("", null)]
        [DataRow(" ", null)]
        [DataRow("  ", null)]
        public void CanRemoveEmptyStrings(string inputData, string expectedData)
        {
            // Arrange
            var classUnderTest = new StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter();
            classUnderTest.Initialize(new CsvConverterCustomAttribute(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter)) { Order = 1});

            //  Act
            string actualData =  classUnderTest.Convert(inputData, ColumnName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
