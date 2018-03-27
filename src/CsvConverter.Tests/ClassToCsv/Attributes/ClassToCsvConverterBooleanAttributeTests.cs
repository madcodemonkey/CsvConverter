using CsvConverter.ClassToCsv;
using CsvConverter.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CsvConverter.Tests.Services
{
    [TestClass]
    public class ClassToCsvConverterBooleanAttributeTests
    {
        [DataTestMethod]
        [DataRow(true, "Yes", "Y", "True", "T", "1")]
        [DataRow(false, "No", "N", "False", "F", "0")]
        public void CanControlTheWayStringsAreOutput_YesOrNoIsWrittenCorrectlyToTheRowWriter(bool inputValue,
            string expectedYesorNo, string expectedYorN, string expectedTrueOrFalse, string expectedTorF, string expectedZeroOrOne)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new ClassToCsvService<ClassToCsvConverterBooleanAttributeData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new ClassToCsvConverterBooleanAttributeData1() { Order = 1, IsYesOrNo = inputValue, IsYOrN = inputValue, IsTOrF = inputValue, IsTrueOrFalse= inputValue, IsZeroOrOne = inputValue };

            // Act
            classUnderTest.WriterRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            Assert.AreEqual("1", dataRow[0]);
            Assert.AreEqual(expectedYesorNo, dataRow[1]);
            Assert.AreEqual(expectedYorN, dataRow[2]);
            Assert.AreEqual(expectedTrueOrFalse, dataRow[3]);
            Assert.AreEqual(expectedTorF, dataRow[4]);
            Assert.AreEqual(expectedZeroOrOne, dataRow[5]);
        }
    }

    internal class ClassToCsvConverterBooleanAttributeData1
    {
        [CsvConverter(ColumnIndex = 1)]
        public int Order { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        [ClassToCsvConverterBoolean(BooleanOutputFormatEnum.UseYesAndNo)]
        public bool IsYesOrNo { get; set; }

        [CsvConverter(ColumnIndex = 3)]
        [ClassToCsvConverterBoolean(BooleanOutputFormatEnum.UseYandN)]
        public bool IsYOrN { get; set; }

        [CsvConverter(ColumnIndex = 4)]
        [ClassToCsvConverterBoolean(BooleanOutputFormatEnum.UseTrueAndFalse)]
        public bool IsTrueOrFalse { get; set; }

        [CsvConverter(ColumnIndex = 5)]
        [ClassToCsvConverterBoolean(BooleanOutputFormatEnum.UseTandF)]
        public bool IsTOrF { get; set; }
        
        [CsvConverter(ColumnIndex = 6)]
        [ClassToCsvConverterBoolean(BooleanOutputFormatEnum.UseOneAndZero)]
        public bool IsZeroOrOne { get; set; }
    }
}
