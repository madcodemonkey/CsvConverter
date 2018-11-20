using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class PropertyLevelAttributeOrderWriteTests
    {
        // 1st converter remove spaces and 2nd converter does an exact match
        [DataTestMethod]
        [DataRow("dog", "cat")]        // 1st converter has no spaces to remove and 2nd converter gets a match
        [DataRow(" dog", "cat")]       // 1st converter should REMOVE spaces and 2nd converter gets a match
        [DataRow(" dog ", "cat")]      // 1st converter should REMOVE spaces and 2nd converter gets a match
        [DataRow("moose", "moose")]    // 1st converter has no spaces to remove and 2nd converter gets no matches
        [DataRow(" moose", "moose")]   // 1st converter removes spaces and 2nd converter gets no matches
        [DataRow(" moose ", "moose")]  // 1st converter removes spaces and 2nd converter gets no matches
        public void Convert_OrderMatters_FirstRemoveSpacesAndThenDoExactMatch(
            string animialTypeInput, string animialTypeExpectedOutput)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<PropLevelAttributeOrderWriteData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new PropLevelAttributeOrderWriteData1() { AnimalType = animialTypeInput };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count); // header row and then 1 data row (count of 2)
            var dataRow = rowWriterMock.Rows[1];  // first row below header row

            Assert.AreEqual(animialTypeExpectedOutput, dataRow[0]);
        }

        // Atrribute Order REVERSED.  1st converter does an exact match  and 2nd converter remove spaces
        [DataTestMethod]
        [DataRow("dog", "cat")]        // 1st converter gets a match and 2nd converter has no spaces to remove 
        [DataRow(" dog", "dog")]       // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow(" dog ", "dog")]      // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow("moose", "moose")]    // 1st converter gets NO match and 2nd converter has no spaces to remove 
        [DataRow(" moose", "moose")]   // 1st converter gets NO match and 2nd converter REMOVES spaces
        [DataRow(" moose ", "moose")]  // 1st converter gets NO match and 2nd converter REMOVES spaces
        public void Convert_OrderMatters_FirstDoExactMatchAndThenRemoveSpaces(
      string animialTypeInput, string animialTypeExpectedOutput)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<PropLevelAttributeOrderWriteData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new PropLevelAttributeOrderWriteData2() { AnimalType = animialTypeInput };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count); // header row and then 1 data row (count of 2)
            var dataRow = rowWriterMock.Rows[1];  // first row below header row

            Assert.AreEqual(animialTypeExpectedOutput, dataRow[0]);
        }

    }


    // EVERY match removes spaces so and EXACT match replaces words.  So ORDER MATTERS here so the exact match and find words.
     internal class PropLevelAttributeOrderWriteData1
    {
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
           OldValue = " ", NewValue = "", Order = 1, IsPostConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
           OldValue = "dog", NewValue = "cat", Order = 2, IsPostConverter = true)]
        public string AnimalType { get; set; }
    }


    // Order reversed so that things do NOT work the same way
    internal class PropLevelAttributeOrderWriteData2
    {
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
           OldValue = " ", NewValue = "", Order = 2, IsPostConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
           OldValue = "dog", NewValue = "cat", Order = 1, IsPostConverter = true)]
        public string AnimalType { get; set; }
    }
}
