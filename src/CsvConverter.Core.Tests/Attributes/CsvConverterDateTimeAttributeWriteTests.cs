using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterDateTimeAttributeWriteTests
    {
        [DataTestMethod]
        [DataRow(2017, 5, 8, 14, 40, 12, "2017-05-08 14:40:12", "Monday, May 08, 2017 2:40 PM", "5/8/2017")]
        public void Convert_WhenSpecifiedOnEachProperty_DatesAreConvertedAccordingToAttributes(int year, int month, int day, int hour, int minute, int second,
            string expectedStringRow1, string expectedStringRow2, string expectedStringRow3)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();

            var classUnderTest = new CsvWriterService<CsvConverterDateTimeWriteData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;



            var data = new CsvConverterDateTimeWriteData1()
            {
                Date1 = new DateTime(year, month, day, hour, minute, second),
                Date2 = new DateTime(year, month, day, hour, minute, second),
                Date3 = new DateTime(year, month, day, hour, minute, second)
            };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(expectedStringRow1, rowWriterMock.LastRow[0], "Order column problem for Date1");
            Assert.AreEqual(expectedStringRow2, rowWriterMock.LastRow[1], "Order column problem for Date2!");
            Assert.AreEqual(expectedStringRow3, rowWriterMock.LastRow[2], "Order column problem for Date3!");
        }


        [DataTestMethod]
        [DataRow(2017, 5, 8, 14, 40, 12, "2017-05-08 14:40:12", "2017-05-08 14:40:12", "2017-05-08 14:40:12")]
        public void Convert_WhenSpecifiedOnTheClass_DatesAreAllConvertedTheSameWay(
            int year, int month, int day, int hour, int minute, int second,
              string expectedStringRow1, string expectedStringRow2, string expectedStringRow3)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();

            var classUnderTest = new CsvWriterService<CsvConverterDateTimeWriteData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;



            var data = new CsvConverterDateTimeWriteData2()
            {
                Date1 = new DateTime(year, month, day, hour, minute, second),
                Date2 = new DateTime(year, month, day, hour, minute, second),
                Date3 = new DateTime(year, month, day, hour, minute, second)
            };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(expectedStringRow1, rowWriterMock.LastRow[0], "Order column problem for Date1");
            Assert.AreEqual(expectedStringRow2, rowWriterMock.LastRow[1], "Order column problem for Date2!");
            Assert.AreEqual(expectedStringRow3, rowWriterMock.LastRow[2], "Order column problem for Date3!");
        }


        [DataTestMethod]
        [DataRow(2017, 5, 8, 14, 40, 12, "2017-05-08 14:40:12", "2017-05-08 14:40:12", "5/8/2017")]
        public void Convert_WhenSpecifiedTheClassPropertyAttributesCanOverrideClassAttributes_PropertiesTakePrecedence(
            int year, int month, int day, int hour, int minute, int second,
       string expectedStringRow1, string expectedStringRow2, string expectedStringRow3)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();

            var classUnderTest = new CsvWriterService<CsvConverterDateTimeWriteData3>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;



            var data = new CsvConverterDateTimeWriteData3()
            {
                Date1 = new DateTime(year, month, day, hour, minute, second),
                Date2 = new DateTime(year, month, day, hour, minute, second),
                Date3 = new DateTime(year, month, day, hour, minute, second)
            };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(expectedStringRow1, rowWriterMock.LastRow[0], "Order column problem for Date1");
            Assert.AreEqual(expectedStringRow2, rowWriterMock.LastRow[1], "Order column problem for Date2!");
            Assert.AreEqual(expectedStringRow3, rowWriterMock.LastRow[2], "Order column problem for Date3!");
        }
    }



    internal class CsvConverterDateTimeWriteData1
    {
        [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss")]
        public DateTime Date1 { get; set; }

        [CsvConverterDateTime(StringFormat = "dddd, MMMM dd, yyyy h:mm tt")]
        public DateTime Date2 { get; set; }

        [CsvConverterDateTime(StringFormat = "M/d/yyyy")]
        public DateTime? Date3 { get; set; }

    }

    [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss", TargetPropertyType = typeof(DateTime))]
    [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss", TargetPropertyType = typeof(DateTime?))]
    internal class CsvConverterDateTimeWriteData2
    {
        public DateTime Date1 { get; set; }

        public DateTime Date2 { get; set; }

        public DateTime? Date3 { get; set; }

    }


    [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss", TargetPropertyType = typeof(DateTime))]
    [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss", TargetPropertyType = typeof(DateTime?))]
    internal class CsvConverterDateTimeWriteData3
    {
        public DateTime Date1 { get; set; }

        public DateTime Date2 { get; set; }

        [CsvConverterDateTime(StringFormat = "M/d/yyyy")]
        public DateTime? Date3 { get; set; }
    }
}
