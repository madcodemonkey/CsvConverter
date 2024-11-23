using CsvConverter.RowTools;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class PropertyLevelAttributeOrderReadTests
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

            var classUnderTest = new CsvReaderService<PropLevelAttributeOrderReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            PropLevelAttributeOrderReadData1 row1 = classUnderTest.GetRecord();
            PropLevelAttributeOrderReadData1 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(animalTypeExpected, row1.AnimalType);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }




        // Attribute Order REVERSED.  1st converter does an exact match  and 2nd converter remove spaces
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

            var classUnderTest = new CsvReaderService<PropLevelAttributeOrderReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            PropLevelAttributeOrderReadData2 row1 = classUnderTest.GetRecord();
            PropLevelAttributeOrderReadData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(animalTypeExpected, row1.AnimalType);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }
    }


    internal class PropLevelAttributeOrderReadData1
    {
        public int Order { get; set; }

        [CsvConverterString(ColumnName = "Animal Type")]
        // EVERY match removes spaces so and EXACT match replaces words.  So ORDER MATTERS here so the exact match and find words.
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
           OldValue = " ", NewValue = "", Order = 1, IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
           OldValue = "dog", NewValue = "cat", Order = 2, IsPreConverter = true)]
        public string AnimalType { get; set; } = string.Empty;
    }


    // Order reversed so that things do NOT work the same way
    internal class PropLevelAttributeOrderReadData2
    {
        public int Order { get; set; }

        [CsvConverterString(ColumnName = "Animal Type")]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
           OldValue = " ", NewValue = "", Order = 2, IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
           OldValue = "dog", NewValue = "cat", Order = 1, IsPreConverter = true)]
        public string AnimalType { get; set; } = string.Empty;
    }
}
