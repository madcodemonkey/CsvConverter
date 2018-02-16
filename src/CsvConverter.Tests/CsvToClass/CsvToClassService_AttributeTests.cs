﻿using System;
using System.Collections.Generic;
using System.Globalization;
using CsvConverter.CsvToClass;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class CsvToClassService_AttributeTests
    {
        [TestMethod]
        public void CanSpecifyAnotherColumnName()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "FirstName", "LastName" })
                .Returns(new List<string> { "1", "John", "Adams" })
                .Returns(new List<string> { "2", "Bob", "Hope" })
                .Returns(new List<string> { "3", "James", "Garner" });

            var classUnderTest = new CsvToClassService<CsvServiceAttributeTestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceAttributeTestClass row1 = classUnderTest.GetRecord();
            CsvServiceAttributeTestClass row2 = classUnderTest.GetRecord();
            CsvServiceAttributeTestClass row3 = classUnderTest.GetRecord();
            CsvServiceAttributeTestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual("John", row1.First);
            Assert.AreEqual("Adams", row1.Last);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Bob", row2.First);
            Assert.AreEqual("Hope", row2.Last);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("James", row3.First);
            Assert.AreEqual("Garner", row3.Last);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void CanSpecifySecondaryColumnName()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "FirstOne", "LastOne" })
                .Returns(new List<string> { "1", "John", "Adams" })
                .Returns(new List<string> { "2", "Bob", "Hope" })
                .Returns(new List<string> { "3", "James", "Garner" });

            var classUnderTest = new CsvToClassService<CsvServiceAttribute2TestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceAttribute2TestClass row1 = classUnderTest.GetRecord();
            CsvServiceAttribute2TestClass row2 = classUnderTest.GetRecord();
            CsvServiceAttribute2TestClass row3 = classUnderTest.GetRecord();
            CsvServiceAttribute2TestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual("John", row1.First);
            Assert.AreEqual("Adams", row1.Last);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Bob", row2.First);
            Assert.AreEqual("Hope", row2.Last);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("James", row3.First);
            Assert.AreEqual("Garner", row3.Last);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void CanHandleSpecializedAttributes()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "BirthDay", "PercentageBodyFat", "PercentageMuscle", "Length", "LengthArms" })
                .Returns(new List<string> { "1", "2017-05-08 14:40:12", "34.56789", "78.33212", "98.34222", "67.94783" })
                .Returns(new List<string> { "2", "2018-05-27 14:40:13", "67.89004", "79.33212", "87.38278", "68.94783" })
                .Returns(new List<string> { "3", "1999-01-01 14:40:24", "948.5334", "80.33212", "7645.322", "69.94783" });

            var classUnderTest = new CsvToClassService<CsvServiceAttribute3TestClass>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvServiceAttribute3TestClass row1 = classUnderTest.GetRecord();
            CsvServiceAttribute3TestClass row2 = classUnderTest.GetRecord();
            CsvServiceAttribute3TestClass row3 = classUnderTest.GetRecord();
            CsvServiceAttribute3TestClass row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(new DateTime(2017, 5, 8, 14, 40, 12), row1.BirthDay);
            Assert.AreEqual(34.57m, row1.PercentageBodyFat);
            Assert.AreEqual(78.3m, row1.PercentageMuscle);
            Assert.AreEqual(98.342, row1.Length);
            Assert.AreEqual(67.9478, row1.LengthArms);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(new DateTime(2018, 5, 27, 14, 40, 13), row2.BirthDay);
            Assert.AreEqual(67.89m, row2.PercentageBodyFat);
            Assert.AreEqual(79.3m, row2.PercentageMuscle);
            Assert.AreEqual(87.383, row2.Length);
            Assert.AreEqual(68.9478, row2.LengthArms);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual(new DateTime(1999, 1, 1, 14, 40, 24), row3.BirthDay);
            Assert.AreEqual(80.3m, row3.PercentageMuscle);
            Assert.AreEqual(948.53m, row3.PercentageBodyFat);
            Assert.AreEqual(7645.322, row3.Length);
            Assert.AreEqual(69.9478, row3.LengthArms);

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

    }

    internal class CsvServiceAttributeTestClass
    {
        public int Order { get; set; }

        [CsvConverter(ColumnName = "FirstName")]
        public string First { get; set; }

        [CsvConverter(ColumnName = "LastName")]
        public string Last { get; set; }

        [CsvConverter(IgnoreWhenReading = true)]
        public int Age { get; set; }
    }

    internal class CsvServiceAttribute2TestClass
    {
        public int Order { get; set; }

        [CsvConverter(AltColumnNames = "First1,FirstOne")]
        public string First { get; set; }

        [CsvConverter(AltColumnNames = "Last1,LastOne")]
        public string Last { get; set; }

        [CsvConverter(IgnoreWhenReading = true)]
        public int Age { get; set; }
    }

    internal class CsvServiceAttribute3TestClass
    {
        public int Order { get; set; }

        [CsvToClassDateTimeTypeConverter(DateParseExactFormat = "yyyy-MM-dd HH:mm:ss", DateStyle = DateTimeStyles.None)]
        public DateTime BirthDay { get; set; }

        [CsvToClassDecimalTypeConverter(NumberOfDecimalPlaces = 2)]
        public decimal PercentageBodyFat { get; set; }

        [CsvToClassDecimalTypeConverter(NumberOfDecimalPlaces = 1)]
        public decimal PercentageMuscle { get; set; }

        [CsvToClassDoubleTypeConverter(NumberOfDecimalPlaces = 3)]
        public double Length { get; set; }

        [CsvToClassDoubleTypeConverter(NumberOfDecimalPlaces = 4)]
        public double LengthArms { get; set; }
    }

}
