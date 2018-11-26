using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterNumber_IntegerWriteTests
    {
        [DataTestMethod]
        [DataRow(2000, "2,000", 2000, "2,000.0", 2000, "$2,000.00", 1, "100%")]
        [DataRow(1234, "1,234", 1234, "1,234.0", 1234, "$1,234.00", 2, "200%")]
        [DataRow(5678, "5,678", 5678, "5,678.0", 5678, "$5,678.00", 3, "300%")]
        public void Convert_WhenSpecifiedOnEachProperty_IntsAreConvertedAccordingToAttributes(
            int inputData1, string expectedData1,
            int inputData2, string expectedData2,
            int? inputData3, string expectedData3,
            int inputData4, string expectedData4)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterNumberIntWriteData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterNumberIntWriteData1() { Num1 = inputData1, Num2 = inputData2, Num3 = inputData3, Num4 = inputData4 };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            var index = 0;

            Assert.AreEqual(expectedData1, dataRow[index++], $"Problem with input {index} -> {inputData1}");
            Assert.AreEqual(expectedData2, dataRow[index++], $"Problem with input {index} -> {inputData2}");
            Assert.AreEqual(expectedData3, dataRow[index++], $"Problem with input {index} -> {inputData3}");

            // Note:  Windows 7 and Windows 10 format percentages in different ways so I'm just remove 
            // the space between the number and percentage sign so that I can test for a proper conversion.
            Assert.AreEqual(expectedData4, dataRow[index++].Replace(" ", ""), $"Problem with input {index} -> {inputData4}");
        }

        [DataTestMethod]
        [DataRow(2000, "2,000", 2000, "2,000", 2000, "2,000", 2000, "2,000")]
        [DataRow(1234, "1,234", 1234, "1,234", 1234, "1,234", 1234, "1,234")]
        [DataRow(5678, "5,678", 5678, "5,678", 5678, "5,678", 5678, "5,678")]
        public void Convert_WhenSpecifiedOnTheClass_IntsAreAllConvertedTheSameWay(
           int inputData1, string expectedData1,
           int inputData2, string expectedData2,
           int? inputData3, string expectedData3,
           int inputData4, string expectedData4)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterNumberIntWriteData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterNumberIntWriteData2() { Num1 = inputData1, Num2 = inputData2, Num3 = inputData3, Num4 = inputData4 };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            var index = 0;

            Assert.AreEqual(expectedData1, dataRow[index++], $"Problem with input {index} -> {inputData1}");
            Assert.AreEqual(expectedData2, dataRow[index++], $"Problem with input {index} -> {inputData2}");
            Assert.AreEqual(expectedData3, dataRow[index++], $"Problem with input {index} -> {inputData3}");
            Assert.AreEqual(expectedData4, dataRow[index++], $"Problem with input {index} -> {inputData4}");
        }


        [DataTestMethod]
        [DataRow(2000, "2,000", 2000, "$2,000.00", 2000, "2,000", 2000, "2,000")]
        [DataRow(1234, "1,234", 1234, "$1,234.00", 1234, "1,234", 1234, "1,234")]
        [DataRow(5678, "5,678", 5678, "$5,678.00", 5678, "5,678", 5678, "5,678")]
        public void Convert_WhenSpecifiedTheClassPropertyAttributesCanOverrideClassAttributes_PropertiesTakePrecedence(
            int inputData1, string expectedData1,
            int inputData2, string expectedData2,
            int? inputData3, string expectedData3,
            int inputData4, string expectedData4)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterNumberIntWriteData3>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterNumberIntWriteData3()
            {
                Num1 = inputData1,
                Num2 = inputData2,
                Num3 = inputData3,
                Num4 = inputData4
            };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            var index = 0;

            Assert.AreEqual(expectedData1, dataRow[index++], $"Problem with input {index} -> {inputData1}");
            Assert.AreEqual(expectedData2, dataRow[index++], $"Problem with input {index} -> {inputData2}");
            Assert.AreEqual(expectedData3, dataRow[index++], $"Problem with input {index} -> {inputData3}");
        }
    }


    internal class CsvConverterNumberIntWriteData1
    {
        [CsvConverterNumber(StringFormat = "N0")]
        public int Num1 { get; set; }

        [CsvConverterNumber(StringFormat = "N1")]
        public int Num2 { get; set; }

        [CsvConverterNumber(StringFormat = "C2")]
        public int? Num3 { get; set; }

        [CsvConverterNumber(StringFormat = "P0")]
        public int Num4 { get; set; }
    }

    [CsvConverterNumber(StringFormat = "N0", TargetPropertyType = typeof(int))]
    [CsvConverterNumber(StringFormat = "N0", TargetPropertyType = typeof(int?))]
    internal class CsvConverterNumberIntWriteData2
    {
        public int Num1 { get; set; }

        public int Num2 { get; set; }

        public int? Num3 { get; set; }

        public int Num4 { get; set; }
    }


    [CsvConverterNumber(StringFormat = "N0", TargetPropertyType = typeof(int))]
    [CsvConverterNumber(StringFormat = "N0", TargetPropertyType = typeof(int?))]
    internal class CsvConverterNumberIntWriteData3
    {
        public int Num1 { get; set; }

        [CsvConverterNumber(StringFormat = "C2")]
        public int Num2 { get; set; }

        public int? Num3 { get; set; }

        public int Num4 { get; set; }
    }
}
