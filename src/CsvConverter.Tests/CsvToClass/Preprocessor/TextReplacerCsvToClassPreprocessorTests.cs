using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvConverter.CsvToClass;

namespace CsvConverter.Tests
{
    [TestClass]
    public class TextReplacerCsvToClassPreprocessorTests
    {
        private const string ColumnName = "SomeColumn";
        private const int ColumnIndex = 0;
        private const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("Michael's", "'", "", "Michaels")]
        [DataRow("Michael\"s", "\"", "", "Michaels")]
        [DataRow(null, "a", "", null)]
        [DataRow("", "a", "", "")]
        [DataRow("Michael", "ae", "", "Michl")]
        [DataRow("Michael", "", "", "Michael")]
        [DataRow("Michael", null, "", "Michael")]
        [DataRow("Michael's", "'", "-", "Michael-s")]
        [DataRow("Michael", "chael", "ke", "Mike")]
        public void CanRemoveData(string inputData, string oldValue, string newValue, string expectedData)
        {
            // Arrange
            var classUnderTest = new TextReplacerCsvToClassPreprocessor();
            classUnderTest.Initialize(new CsvConverterOldAndNewValueAttribute(typeof(TextReplacerCsvToClassPreprocessor)) { Order = 1, OldValue = oldValue, NewValue = newValue });

            //  Act
            string actualData =  classUnderTest.Work(inputData, ColumnName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
