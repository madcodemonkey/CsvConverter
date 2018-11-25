using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class ClassLevelAttributeOrderWriteTests
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
            var classUnderTest = new CsvWriterService<ClassLevelAttributeOrderWriteData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassLevelAttributeOrderWriteData1() { AnimalType = animialTypeInput };

            // Act
            classUnderTest.WriteRecord(data);

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
            var classUnderTest = new CsvWriterService<ClassLevelAttributeOrderWriteData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassLevelAttributeOrderWriteData2() { AnimalType = animialTypeInput };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count); // header row and then 1 data row (count of 2)
            var dataRow = rowWriterMock.Rows[1];  // first row below header row

            Assert.AreEqual(animialTypeExpectedOutput, dataRow[0]);
        }

    }


    // EVERY match removes spaces so and EXACT match replaces words.  So ORDER MATTERS here so the exact match and find words.
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 1, IsPostConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
        TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 2, IsPostConverter = true)]
    internal class ClassLevelAttributeOrderWriteData1
    {
        public string AnimalType { get; set; }
    }


    // Order reversed so that things do NOT work the same way
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 2, IsPostConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
        TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 1, IsPostConverter = true)]
    internal class ClassLevelAttributeOrderWriteData2
    {
        public string AnimalType { get; set; }
    }
}
