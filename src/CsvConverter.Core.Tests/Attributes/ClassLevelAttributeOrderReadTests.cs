using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class ClassLevelAttributeOrderReadTests
    {
        // 1st converter remove spaces and 2nd converter does an exact match
        [DataTestMethod]
        [DataRow("1", "dog", "cat")]        // 1st converter has no spaces to remove and 2nd converter gets a match
        [DataRow("1", " dog", "cat")]       // 1st converter should REMOVE spaces and 2nd converter gets a match
        [DataRow("1", " dog ", "cat")]      // 1st converter should REMOVE spaces and 2nd converter gets a match
        [DataRow("1", "moose", "moose")]    // 1st converter has no spaces to remove and 2nd converter gets no matches
        [DataRow("1", " moose", "moose")]   // 1st converter removes spaces and 2nd converter gets no matches
        [DataRow("1", " moose ", "moose")]  // 1st converter removes spaces and 2nd converter gets no matches
        public void Convert_OrderMatters_FirstRemoveSpacesAndThenDoExactMatch(string order,
           string animalTypeInput, string animalTypeExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Animal Type" })
                .Returns(new List<string> { order, animalTypeInput });

            var classUnderTest = new CsvReaderService<CsvToClassOrderData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvToClassOrderData1 row1 = classUnderTest.GetRecord();
            CsvToClassOrderData1 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(animalTypeExpected, row1.AnimalType);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }




        // Atrribute Order REVERSED.  1st converter does an exact match  and 2nd converter remove spaces
        [DataTestMethod]
        [DataRow("1", "dog", "cat")]        // 1st converter gets a match and 2nd converter has no spaces to remove 
        [DataRow("1", " dog", "dog")]       // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow("1", " dog ", "dog")]      // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow("1", "moose", "moose")]    // 1st converter gets NO match and 2nd converter has no spaces to remove 
        [DataRow("1", " moose", "moose")]   // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow("1", " moose ", "moose")]  // 1st converter gets NO match and 2nd converter REMOVES spaces
        public void Convert_OrderMatters_FirstDoExactMatchAndThenRemoveSpaces(string order,
           string animalTypeInput, string animalTypeExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Animal Type" })
                .Returns(new List<string> { order, animalTypeInput });

            var classUnderTest = new CsvReaderService<CsvToClassOrderData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvToClassOrderData2 row1 = classUnderTest.GetRecord();
            CsvToClassOrderData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(animalTypeExpected, row1.AnimalType);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }
    }


    // EVERY match removes spaces so and EXACT match replaces words.  So ORDER MATTERS here so the exact match and find words.
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 1, IsPreConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
        TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 2, IsPreConverter = true)]
    internal class CsvToClassOrderData1
    {
        public int Order { get; set; }

        [CsvConverterString(ColumnName = "Animal Type")]
        public string AnimalType { get; set; }
    }


    // Order reversed so that things do NOT work the same way
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 2, IsPreConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
        TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 1, IsPreConverter = true)]
    internal class CsvToClassOrderData2
    {
        public int Order { get; set; }

        [CsvConverterString(ColumnName = "Animal Type")]
        public string AnimalType { get; set; }
    }
}
