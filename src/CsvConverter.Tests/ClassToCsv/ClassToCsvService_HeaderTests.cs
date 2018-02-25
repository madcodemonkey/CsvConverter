using CsvConverter.ClassToCsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class ClassToCsvService_HeaderTests
    {
        [TestMethod]
        public void WriterRecord_CanWriteColumnInCorrectOrder_DataIsWritenCorrectlyToTheRowWriter()
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassToCsvServiceHeaderData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassToCsvServiceHeaderData1() { Order = 1, Age = 23, Name = "James" };

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


    internal class ClassToCsvServiceHeaderData1
    {
        [CsvConverter(ColumnIndex = 1)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 3)]
        public string Name { get; set; }
    }
}
