using CsvConverter.ClassToCsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class ClassToCsvService_PostConverterTests
    {
        [DataTestMethod]
        [DataRow(0, 1, "James", "", "1", "James")]
        [DataRow(1, 0, "James", "1", "", "James")]
        [DataRow(1, 1, "James", "1", "1", "James")]
        [DataRow(1, 1, "Chuck", "1", "1", "Duck")]
        public void WriterRecord_CanPostConvertFieldsOnProperties_DataIsWritenCorrectlyToTheRowWriter(
            int order, int age, string name, string expectedOrder, string expectedAge, string expectedName)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassToCsvServicePostData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassToCsvServicePostData1() { Order = order, Age = age, Name = name };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(expectedOrder, rowWriterMock.LastRow[0], "Order column problem!");
            Assert.AreEqual(expectedAge, rowWriterMock.LastRow[1], "Age column problem!");
            Assert.AreEqual(expectedName, rowWriterMock.LastRow[2], "Name column problem!");
        }


        [DataTestMethod]
        [DataRow(0, 5, "James", "", "", "James")]
        [DataRow(1, 4, "James", "1", "4", "James")]
        [DataRow(2, 3, "James", "2", "3", "James")]
        [DataRow(3, 2, "James", "3", "2", "James")]
        [DataRow(4, 1, "James", "4", "1", "James")]
        [DataRow(5, 0, "James", "", "", "James")]
        [DataRow(50, 10, "James", "", "1", "James")]
        public void WriterRecord_CanPostConvertFieldsOnTheClass_DataIsWritenCorrectlyToTheRowWriter(
            int order, int age, string name, string expectedOrder, string expectedAge, string expectedName)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassToCsvServicePostData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassToCsvServicePostData2() { Order = order, Age = age, Name = name };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(expectedOrder, rowWriterMock.LastRow[0], "Order column problem!");
            Assert.AreEqual(expectedAge, rowWriterMock.LastRow[1], "Age column problem!");
            Assert.AreEqual(expectedName, rowWriterMock.LastRow[2], "Name column problem!");
        }
    }
    
    internal class ClassToCsvServicePostData1
    {
        [CsvConverter(ColumnIndex = 1)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 3)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "Ch", NewValue = "D")]
        public string Name { get; set; }
    }


    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null, TargetPropertyType = typeof(int), Order = 1)]
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "5", NewValue = null, TargetPropertyType = typeof(int), Order = 2)]
    internal class ClassToCsvServicePostData2
    {
        [CsvConverter(ColumnIndex = 1)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 3)]
        public string Name { get; set; }
    }
}
