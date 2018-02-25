using System;
using CsvConverter.ClassToCsv;
using CsvConverter.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests
{
    [TestClass]
    public class TrimClassToCsvTypeConverterTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("test", "test")]
        [DataRow(" test", "test")]
        [DataRow("test ", "test")]
        [DataRow(" test ", "test")]
        [DataRow("   test   ", "test")]
        public void Convert_CanTrimProperties_PropertyTrimmed(string inputData, string expectedData)
        {
            // Arrange
            var data = new TrimClassToCsvTypeTestData();
            data.MyStringProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TrimClassToCsvTypeTestData>(nameof(TrimClassToCsvTypeTestData.MyStringProperty));

            var classUnderTest = new TrimClassToCsvTypeConverter();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.MyStringProperty, null, ColumName, ColumnIndex, RowNumber, null);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
