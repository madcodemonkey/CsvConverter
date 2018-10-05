using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterStringTrimmerTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        const int RowNumber = 1;

        [DataTestMethod]
        [DataRow("test", "test", CsvConverterTrimEnum.All)]
        [DataRow(" test", "test", CsvConverterTrimEnum.All)]
        [DataRow("test ", "test", CsvConverterTrimEnum.All)]
        [DataRow(" test ", "test", CsvConverterTrimEnum.All)]
        [DataRow("   test   ", "test", CsvConverterTrimEnum.All)]
        [DataRow(" ", "", CsvConverterTrimEnum.All)]
        [DataRow("test", "test", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" test", "test", CsvConverterTrimEnum.TrimStart)]
        [DataRow("test ", "test ", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" test ", "test ", CsvConverterTrimEnum.TrimStart)]
        [DataRow("   test   ", "test   ", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" ", "", CsvConverterTrimEnum.TrimStart)]
        [DataRow("test", "test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" test", " test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow("test ", "test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" test ", " test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow("   test   ", "   test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" ", "", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" ", "", CsvConverterTrimEnum.TrimEnd)]
        public void GetReadData_CanTrimProperties_PropertyTrimmed(string inputData, string expectedData, CsvConverterTrimEnum trimAction)
        {
            // Arrange        
            var cut = new CsvConverterStringTrimmer();
            cut.TrimAction = trimAction;

            // Act
            object actualData = cut.GetReadData(typeof(string), inputData, ColumName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow("test", "test", CsvConverterTrimEnum.All)]
        [DataRow(" test", "test", CsvConverterTrimEnum.All)]
        [DataRow("test ", "test", CsvConverterTrimEnum.All)]
        [DataRow(" test ", "test", CsvConverterTrimEnum.All)]
        [DataRow("   test   ", "test", CsvConverterTrimEnum.All)]
        [DataRow(" ", "", CsvConverterTrimEnum.All)]
        [DataRow("test", "test", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" test", "test", CsvConverterTrimEnum.TrimStart)]
        [DataRow("test ", "test ", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" test ", "test ", CsvConverterTrimEnum.TrimStart)]
        [DataRow("   test   ", "test   ", CsvConverterTrimEnum.TrimStart)]
        [DataRow(" ", "", CsvConverterTrimEnum.TrimStart)]
        [DataRow("test", "test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" test", " test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow("test ", "test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" test ", " test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow("   test   ", "   test", CsvConverterTrimEnum.TrimEnd)]
        [DataRow(" ", "", CsvConverterTrimEnum.TrimEnd)]
        public void GetWriteData_CanTrimProperties_PropertyTrimmed(string inputData, string expectedData, CsvConverterTrimEnum trimAction)
        {
            // Arrange        
            var cut = new CsvConverterStringTrimmer();
            cut.TrimAction = trimAction;

            // Act
            string actualData = cut.GetWriteData(typeof(string), inputData, ColumName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }

}
