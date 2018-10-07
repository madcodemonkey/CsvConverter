using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.HeaderTests
{
    [TestClass]
    public class Write_NoHeaderTests
    {   
        [DataTestMethod]
        [DataRow(0, 5, "James", "0", "5", "James")]
        [DataRow(1, 4, "James", "1", "4", "James")]
        [DataRow(2, 3, "James", "2", "3", "James")]
        [DataRow(3, 2, "James", "3", "2", "James")]
        [DataRow(4, 1, "James", "4", "1", "James")]
        [DataRow(5, 0, "James", "5", "0", "James")]
        [DataRow(50, 10, "James", "50", "10", "James")]
        public void WriterRecord_CanWriteDataInCorrectOrderWithNoHeaderRow_DataIsWritenCorrectlyToTheRowWriter(
            int order, int age, string name, string expectedOrder, string expectedAge, string expectedName)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<WriteNoHeaderTestsData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = false;

            var data = new WriteNoHeaderTestsData1() { Order = order, Age = age, Name = name };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(1, rowWriterMock.Rows.Count, "There should only be one row written!");
            Assert.AreEqual(expectedName, rowWriterMock.LastRow[0], "Name column problem!");
            Assert.AreEqual(expectedOrder, rowWriterMock.LastRow[1], "Order column problem!");
            Assert.AreEqual(expectedAge, rowWriterMock.LastRow[2], "Age column problem!");
        }
    }

    internal class WriteNoHeaderTestsData1
    {
        [CsvConverterNumber(ColumnIndex = 2)]
        public int Order { get; set; }

        [CsvConverterNumber(ColumnIndex = 3)]
        public int Age { get; set; }

        [CsvConverterString(ColumnIndex = 1)]
        public string Name { get; set; }
    }
}
