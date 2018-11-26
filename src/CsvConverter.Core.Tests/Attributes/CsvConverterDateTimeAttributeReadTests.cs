using System;
using System.Collections.Generic;
using System.Globalization;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterDateTimeAttributeReadTests
    {
        [TestMethod]
        public void GetRecord_ConvertDatesOfVariousFormats()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "SomeDate1", "SomeDate2", "SomeDate3", "SomeDate4", "SomeDate5" })
                .Returns(new List<string> { "1", "2017-05-08 14:40:12", "5/8/2017", "Sunday, March 09, 2008", "3/9/2008 4:05 PM", "March, 2008" })
                .Returns(new List<string> { "2", "2018-05-27 08:50:18", "6/14/2019", "Saturday, May 05, 2018", "8/11/2018 5:15 PM", "June, 2018" })
                .Returns(new List<string> { "3", "2016-09-08 18:21:02", "9/18/2011", "Wednesday, March 19, 2008", "8/9/2008 4:05 PM", "January, 2014" });

            var classUnderTest = new CsvReaderService<CsvConverterDateTimeReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterDateTimeReadData1 row1 = classUnderTest.GetRecord();
            CsvConverterDateTimeReadData1 row2 = classUnderTest.GetRecord();
            CsvConverterDateTimeReadData1 row3 = classUnderTest.GetRecord();
            CsvConverterDateTimeReadData1 row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(new DateTime(2017, 5, 8, 14, 40, 12), row1.SomeDate1);
            Assert.AreEqual(new DateTime(2017, 5, 8, 00, 00, 00), row1.SomeDate2);
            Assert.AreEqual(new DateTime(2008, 3, 9, 00, 00, 00), row1.SomeDate3);
            Assert.AreEqual(new DateTime(2008, 3, 9, 16, 05, 00), row1.SomeDate4);
            Assert.AreEqual(new DateTime(2008, 3, 1, 00, 00, 00), row1.SomeDate5);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(new DateTime(2018, 5, 27, 8, 50, 18), row2.SomeDate1);
            Assert.AreEqual(new DateTime(2019, 6, 14, 00, 00, 00), row2.SomeDate2);
            Assert.AreEqual(new DateTime(2018, 5, 5, 00, 00, 00), row2.SomeDate3);
            Assert.AreEqual(new DateTime(2018, 8, 11, 17, 15, 00), row2.SomeDate4);
            Assert.AreEqual(new DateTime(2018, 6, 1, 00, 00, 00), row2.SomeDate5);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(new DateTime(2016, 9, 8, 18, 21, 02), row3.SomeDate1);
            Assert.AreEqual(new DateTime(2011, 9, 18, 00, 00, 00), row3.SomeDate2);
            Assert.AreEqual(new DateTime(2008, 3, 19, 00, 00, 00), row3.SomeDate3);
            Assert.AreEqual(new DateTime(2008, 8, 9, 16, 05, 00), row3.SomeDate4);
            Assert.AreEqual(new DateTime(2014, 1, 1, 00, 00, 00), row3.SomeDate5);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }
        // 
        [DataTestMethod]
        [DataRow("1", "5/8/2017", 5, 8, 2017, "5/18/2018", 5, 18, 2018)]
        public void GetRecord_WhenClassAttributeIsUsedDatesAreConvertedTheSameWay_ValuesConverted(string order,
            string someDate1, int month1, int day1, int year1,
            string someDate2, int month2, int day2, int year2)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "SomeDate1", "SomeDate2" })
                .Returns(new List<string> { order, someDate1, someDate2 });

            var classUnderTest = new CsvReaderService<CsvConverterDateTimeReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterDateTimeReadData2 row1 = classUnderTest.GetRecord();
            CsvConverterDateTimeReadData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(year1, row1.SomeDate1.Year);
            Assert.AreEqual(month1, row1.SomeDate1.Month);
            Assert.AreEqual(day1, row1.SomeDate1.Day);
            Assert.AreEqual(year2, row1.SomeDate2.Value.Year);
            Assert.AreEqual(month2, row1.SomeDate2.Value.Month);
            Assert.AreEqual(day2, row1.SomeDate2.Value.Day);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", "5/8/2017", 5, 8, 2017, "2018/5/18", 5, 18, 2018)]
        public void GetRecord_WhenClassAttributeIsUsedYouCanOverrideIndividualProperties__ValuesConverted(string order,
            string someDate1, int month1, int day1, int year1,
            string someDate2, int month2, int day2, int year2)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "SomeDate1", "SomeDate2" })
                .Returns(new List<string> { order, someDate1, someDate2 });

            var classUnderTest = new CsvReaderService<CsvConverterDateTimeReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterDateTimeReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterDateTimeReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(year1, row1.SomeDate1.Year);
            Assert.AreEqual(month1, row1.SomeDate1.Month);
            Assert.AreEqual(day1, row1.SomeDate1.Day);
            Assert.AreEqual(year2, row1.SomeDate2.Value.Year);
            Assert.AreEqual(month2, row1.SomeDate2.Value.Month);
            Assert.AreEqual(day2, row1.SomeDate2.Value.Day);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

    }

    internal class CsvConverterDateTimeReadData1
    {
        public int Order { get; set; }

        [CsvConverterDateTime(StringFormat = "yyyy-MM-dd HH:mm:ss", DateStyle = DateTimeStyles.None)]
        public DateTime SomeDate1 { get; set; }

        [CsvConverterDateTime(StringFormat = "M/d/yyyy", DateStyle = DateTimeStyles.None)]
        public DateTime SomeDate2 { get; set; }

        [CsvConverterDateTime(StringFormat = "dddd, MMMM dd, yyyy", DateStyle = DateTimeStyles.None)]
        public DateTime SomeDate3 { get; set; }

        [CsvConverterDateTime(StringFormat = "M/d/yyyy h:mm tt", DateStyle = DateTimeStyles.None)]
        public DateTime SomeDate4 { get; set; }

        [CsvConverterDateTime(StringFormat = "MMMM, yyyy", DateStyle = DateTimeStyles.None)]
        public DateTime SomeDate5 { get; set; }
    }

    [CsvConverterDateTime(StringFormat = "M/d/yyyy", DateStyle = DateTimeStyles.None, TargetPropertyType = typeof(DateTime))]
    [CsvConverterDateTime(StringFormat = "M/d/yyyy", DateStyle = DateTimeStyles.None, TargetPropertyType = typeof(DateTime?))]
    internal class CsvConverterDateTimeReadData2
    {
        public int Order { get; set; }

        public DateTime SomeDate1 { get; set; }

        public DateTime? SomeDate2 { get; set; }
    }

    [CsvConverterDateTime(StringFormat = "M/d/yyyy", DateStyle = DateTimeStyles.None, TargetPropertyType = typeof(DateTime))]
    [CsvConverterDateTime(StringFormat = "M/d/yyyy", DateStyle = DateTimeStyles.None, TargetPropertyType = typeof(DateTime?))]
    internal class CsvConverterDateTimeReadData3
    {
        public int Order { get; set; }

        public DateTime SomeDate1 { get; set; }

        [CsvConverterDateTime(StringFormat = "yyyy/M/d", DateStyle = DateTimeStyles.None)]
        public DateTime? SomeDate2 { get; set; }
    }
}
