using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultDateTimeTests
    {
        [DataTestMethod]
        [DataRow(2018, 5, 2, 9, 51, 2, null, "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "", "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "  ", "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "s", "2018-05-02T09:51:02")]
        // [DataRow(2018, 5, 2, 9, 51, 2, "m", "May 2")]  // Differs from Windows 7 (May 02) to Windows 10 (May 2)
        public void GetWriteData_CanConvertDateTime_DateTimeConverted(
            int year, int month, int day,
            int hour, int minute, int second,
            string dateFormat, string expectedData)
        {
            // Arrange
            var inputValue = new DateTime(year, month, day, hour, minute, second);

            var cut = new CsvConverterDefaultDateTime();
            cut.DateFormat = dateFormat;

            // Act
            string actualData = cut.GetWriteData(typeof(DateTime), inputValue, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0, 0, null, null)]
        [DataRow(2018, 5, 2, 9, 51, 2, null, "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "", "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "  ", "5/2/2018 9:51:02 AM")]
        [DataRow(2018, 5, 2, 9, 51, 2, "s", "2018-05-02T09:51:02")]
        // [DataRow(2018, 5, 2, 9, 51, 2, "m", "May 2")]  // Differs from Windows 7 (May 02) to Windows 10 (May 2)
        public void ConvertGetWriteData_CanConvertNullableDateTime_DateTimeConverted(
         int year, int month, int day,
         int hour, int minute, int second,
         string dateFormat, string expectedData)
        {
            // Arrange
            DateTime? inputValue = year > 0 ? new DateTime(year, month, day, hour, minute, second) : (DateTime?)null;

            var cut = new CsvConverterDefaultDateTime();
            cut.DateFormat = dateFormat;

            // Act
            string actualData = cut.GetWriteData(typeof(DateTime?), inputValue, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }


        [DataTestMethod]
        [DataRow(2017, 5, 6, 0, 0, 0, "yyyyMMdd", "20170506")]
        [DataRow(0001, 1, 1, 0, 0, 0, "yyyyMMdd", "")]
        [DataRow(0001, 1, 1, 0, 0, 0, "yyyyMMdd", "   ")]
        [DataRow(2008, 3, 9, 0, 0, 0, "M/d/yyyy", "3/9/2008")]
        [DataRow(2008, 3, 9, 0, 0, 0, "dddd, MMMM dd, yyyy", "Sunday, March 09, 2008")]
        [DataRow(2008, 3, 9, 0, 0, 0, "MMMM dd, yyyy", "March 09, 2008")]
        [DataRow(2008, 3, 9, 16, 5, 7, "dddd, MMMM dd, yyyy h:mm:ss tt", "Sunday, March 09, 2008 4:05:07 PM")]
        [DataRow(2008, 3, 9, 16, 5, 0, "M/d/yyyy h:mm tt", "3/9/2008 4:05 PM")]
        [DataRow(2008, 3, 1, 0, 0, 0, "MMMM, yyyy", "March, 2008")]
        [DataRow(2014, 9, 1, 0, 0, 0, "", "41883")]  // An OLE Automation date (You see these when converting Excel dates to regular dates)
        [DataRow(2014, 11, 1, 0, 0, 0, "", "41944")]  // An OLE Automation date (You see these when converting Excel dates to regular dates)
        [DataRow(2016, 11, 1, 0, 0, 0, "", "42675")]  // An OLE Automation date (You see these when converting Excel dates to regular dates)
        [DataRow(2017, 6, 14, 0, 0, 0, "", "42900")]  // An OLE Automation date (You see these when converting Excel dates to regular dates)
        [DataRow(2017, 6, 15, 0, 0, 0, "", "42901")]  // An OLE Automation date (You see these when converting Excel dates to regular dates)
        public void GetReadData_CanConvertDatesWithFormat_ValuesConverted(
           int expectedYear, int expectedMonth, int expectedDay,
           int expectedHour, int expectedMinute, int expectedSeconds,
           string dateParseExactFormat, string inputData)
        {
            // Arrange
            var attribute = new CsvConverterDateTimeAttribute();
            attribute.StringFormat = dateParseExactFormat;

            var cut = new CsvConverterDefaultDateTime();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            DateTime actual = (DateTime)cut.GetReadData(typeof(DateTime), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedYear, actual.Year);
            Assert.AreEqual(expectedMonth, actual.Month);
            Assert.AreEqual(expectedDay, actual.Day);
            Assert.AreEqual(expectedHour, actual.Hour);
            Assert.AreEqual(expectedMinute, actual.Minute);
            Assert.AreEqual(expectedSeconds, actual.Second);
        }

        [DataTestMethod]
        [DataRow(16, 5, 0, "h:mm tt", "4:05 PM")]
        [DataRow(16, 5, 7, "h:mm:ss tt", "4:05:07 PM")]
        public void GetReadData_CanConvertTimesWithFormat_ValuesConverted(
            int expectedHour, int expectedMinute, int expectedSeconds,
            string dateParseExactFormat, string inputData)
        {
            // Arrange
            var attribute = new CsvConverterDateTimeAttribute();
            attribute.StringFormat = dateParseExactFormat;

            var cut = new CsvConverterDefaultDateTime();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            DateTime actual = (DateTime)cut.GetReadData(typeof(DateTime), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedHour, actual.Hour);
            Assert.AreEqual(expectedMinute, actual.Minute);
            Assert.AreEqual(expectedSeconds, actual.Second);
        }


        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("5488e4$#@#")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetReadData_CannotHandleNonDateStrings_ThrowsException(string inputData)
        {
            // Arrange
            var cut = new CsvConverterDefaultDateTime();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            DateTime actual = (DateTime)cut.GetReadData(typeof(DateTime), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }



    }

}
