using System;
using CsvConverter.CsvToClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassToCsv.Tests.CsvToClass.Converters.DefaultTypeConverters
{
    [TestClass]
    public class StringToObjectDateTimeTypeConverterTests
    {
        [DataTestMethod]
        [DataRow(2017, 5, 6, "yyyyMMdd", "20170506")]
        public void DateTime_WithFormat_ValuesAssigned(int year, int month, int day, string dateParseExactFormat, string inputData)
        {
            // Arrange
            var attribute = new CsvToClassConverterDateTimeAttribute();
            attribute.DateParseExactFormat = dateParseExactFormat;
            
            var cut = new StringToObjectDateTimeTypeConverter();
            cut.Initialize(attribute);

            // Act
            DateTime actual = (DateTime) cut.Convert(typeof(DateTime), inputData, "Column1", 1, 1, null);

            // Assert
            Assert.AreEqual(year, actual.Year);
            Assert.AreEqual(month, actual.Month);
            Assert.AreEqual(day, actual.Day);
        }
        

    }
}
