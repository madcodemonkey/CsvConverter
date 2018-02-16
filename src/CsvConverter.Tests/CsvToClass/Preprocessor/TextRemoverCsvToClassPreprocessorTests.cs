using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvConverter.CsvToClass;

namespace CsvConverter.Tests
{
    [TestClass]
    public class TextRemoverCsvToClassPreprocessorTests
    {
        private const string ColumnName = "SomeColumn";
        private const int ColumnIndex = 0;
        private const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("Michael's", "'", "Michaels")]
        [DataRow("Michael\"s", "\"", "Michaels")]
        [DataRow(null, "a", null)]
        [DataRow("", "a", "")]
        [DataRow("Michael", "ae", "Michl")]
        [DataRow("Michael", "", "Michael")]
        [DataRow("Michael", null, "Michael")]
        public void CanRemoveData(string inputData, string whatToRemove, string expectedData)
        {
            // Arrange
            var classUnderTest = new TextRemoverCsvToClassPreprocessor();
            classUnderTest.Initialize(new CsvToClassPreprocessorAttribute(typeof(TextRemoverCsvToClassPreprocessor)) { Order = 1, StringInput = whatToRemove });

            //  Act
            string actualData =  classUnderTest.Work(inputData, ColumnName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
