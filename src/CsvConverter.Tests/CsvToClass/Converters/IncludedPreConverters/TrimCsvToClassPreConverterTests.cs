using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvConverter.CsvToClass;

namespace CsvConverter.Tests
{
    [TestClass]
    public class TrimCsvToClassPreConverterTests
    {
        private const string ColumnName = "SomeColumn";
        private const int ColumnIndex = 0;
        private const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("Michael", "Michael")]
        [DataRow(" Michael", "Michael")]
        [DataRow("Michael ", "Michael")]
        [DataRow(" Michael ", "Michael")]
        [DataRow(" ", "")]
        public void CanTrimData(string inputData, string expectedData)
        {
            // Arrange
            var classUnderTest = new TrimCsvToClassPreConverter();
            classUnderTest.Initialize(new CsvConverterOldAndNewValueAttribute(typeof(TrimCsvToClassPreConverter)) { Order = 1 });

            //  Act
            string actualData =  classUnderTest.Work(inputData, ColumnName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
