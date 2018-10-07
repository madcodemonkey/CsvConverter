using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.HeaderTests
{
    [TestClass]
    public class Write_HeaderTests
    {
        [TestMethod]
        public void WriterRecord_CanWriteColumnInCorrectOrder_DataIsWritenCorrectlyToTheRowWriter()
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<WriteHeaderTestData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new WriteHeaderTestData1() { Order = 1, Age = 23, Name = "James" };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var headerRow = rowWriterMock.Rows[0];

            Assert.AreEqual("Order", headerRow[0]);
            Assert.AreEqual("Age", headerRow[1]);
            Assert.AreEqual("Name", headerRow[2]);
        }
    }


    internal class WriteHeaderTestData1
    {
        [CsvConverterNumber(ColumnIndex = 1)]
        public int Order { get; set; }

        [CsvConverterNumber(ColumnIndex = 2)]
        public int Age { get; set; }

        [CsvConverterString(ColumnIndex = 3)]
        public string Name { get; set; }
    }
}
