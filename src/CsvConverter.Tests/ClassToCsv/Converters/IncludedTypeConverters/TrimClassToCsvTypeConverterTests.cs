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
        public void CanTrimProperties(string inputData, string expectedData)
        {
            // Arrange
            var data = new TrimClassToCsvTypeTester();
            data.MyStringProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TrimClassToCsvTypeTester>(nameof(TrimClassToCsvTypeTester.MyStringProperty));

            var classUnderTest = new TrimClassToCsvTypeConverter();


            // object obj, PropertyInfo propInfo, string stringFormat, string columnName, int columnIndex, 
            //int rowNumber, IPropertyToStringConverter defaultConverter
            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.MyStringProperty, null, ColumName, ColumnIndex, RowNumber, null);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }

    internal class TrimClassToCsvTypeTester
    {
        public string MyStringProperty { get; set; }

    }
}
