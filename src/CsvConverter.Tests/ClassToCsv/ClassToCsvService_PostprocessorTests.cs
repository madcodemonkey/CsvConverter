using System.Collections.Generic;
using CsvConverter.ClassToCsv;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class ClassToCsvService_PostprocessorTests
    {
        [DataTestMethod]
        [DataRow(0, 1, "James", "", "1", "James")]
        [DataRow(1, 0, "James", "1", "", "James")]
        [DataRow(1, 1, "James", "1", "1", "James")]
        [DataRow(1, 1, "Chuck", "1", "1", "Duck")]
        public void CanPostProcessFieldsOnProperties(int order, int age, string name, string expectedOrder, string expectedAge, string expectedName)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassTpCsvServicePostProcessorTester1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data1 = new ClassTpCsvServicePostProcessorTester1() { Order = order, Age = age, Name = name };

            // Act
            classUnderTest.WriterRecord(data1);

            // Assert
            Assert.AreEqual(expectedOrder, rowWriterMock.WriteList[0], "Order column problem!");
            Assert.AreEqual(expectedAge, rowWriterMock.WriteList[1], "Age column problem!");
            Assert.AreEqual(expectedName, rowWriterMock.WriteList[2], "Name column problem!");
        }


        [DataTestMethod]
        [DataRow(0, 5, "James", "", "", "James")]
        [DataRow(1, 4, "James", "1", "4", "James")]
        [DataRow(2, 3, "James", "2", "3", "James")]
        [DataRow(3, 2, "James", "3", "2", "James")]
        [DataRow(4, 1, "James", "4", "1", "James")]
        [DataRow(5, 0, "James", "", "", "James")]
        [DataRow(50, 10, "James", "", "1", "James")]
        public void CanPostProcessFieldsOnTheClass(int order, int age, string name, string expectedOrder, string expectedAge, string expectedName)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassTpCsvServicePostProcessorTester2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data1 = new ClassTpCsvServicePostProcessorTester2() { Order = order, Age = age, Name = name };

            // Act
            classUnderTest.WriterRecord(data1);

            // Assert
            Assert.AreEqual(expectedOrder, rowWriterMock.WriteList[0], "Order column problem!");
            Assert.AreEqual(expectedAge, rowWriterMock.WriteList[1], "Age column problem!");
            Assert.AreEqual(expectedName, rowWriterMock.WriteList[2], "Name column problem!");
        }
    }

    internal class FakeRowWriter : IRowWriter
    {
        public int RowNumber { get; set; } = 1;

        public string WriteString { get; set; }
        public List<string> WriteList { get; set; }

        public void Write(List<string> fieldList)
        {
            WriteList = fieldList;
        }

        public void Write(string line)
        {
            WriteString = line;
        }
    }
    
    internal class ClassTpCsvServicePostProcessorTester1
    {
        [CsvConverter(ColumnIndex = 0)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchPostProcessor), OldValue = "0", NewValue = null)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 1)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchPostProcessor), OldValue = "0", NewValue = null)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchPostProcessor), OldValue = "Ch", NewValue = "D")]
        public string Name { get; set; }
    }


    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchPostProcessor), OldValue = "0", NewValue = null, TargetPropertyType = typeof(int), Order = 1)]
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchPostProcessor), OldValue = "5", NewValue = null, TargetPropertyType = typeof(int), Order = 2)]
    internal class ClassTpCsvServicePostProcessorTester2
    {
        [CsvConverter(ColumnIndex = 0)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 1)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        public string Name { get; set; }
    }
}
