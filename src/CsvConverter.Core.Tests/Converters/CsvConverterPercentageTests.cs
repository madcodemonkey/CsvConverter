namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterPercentageTests
    {
        const string ColumName = "Column1";
        const int ColumnIndex = 0;
        const int RowNumber = 1;

        [TestMethod]
        public void GetReadData_CannotCovertJunkStrings()
        {
            // Arrange & Act & Assert
            Assert.Throws<CsvConverterAttributeException>(() =>
            {
                var classUnderTest = new CsvConverterPercentage();
                classUnderTest.Initialize(null, new DefaultTypeConverterFactory());
                classUnderTest.GetReadData(typeof(decimal), "3d5%", ColumName, ColumnIndex, RowNumber);
            });
        }

        // Using same data as below, I just switch the function parameters around
        [TestMethod]
        [DataRow("35%", ".35", 2)]
        [DataRow("35.34%", ".3534", 4)]
        [DataRow("35.123%", ".35123", 5)]
        [DataRow("0.35", ".0035", 4)]
        [DataRow("35", ".35", 2)]
        public void GetReadData_CanCovertStringWithPercentageSign(string inputData, string expected,
            int numberOfDecimalPlaces)
        {
            // Arrange
            var attribute = new CsvConverterNumberAttribute();
            attribute.NumberOfDecimalPlaces = numberOfDecimalPlaces;

            var classUnderTest = new CsvConverterPercentage();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            decimal expectedResult = decimal.Parse(expected);

            // Act
            var actual = classUnderTest.GetReadData(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        // Using same data as above, I just switch the function parameters around
        // and you'll always get a percentage sign on output!
        [TestMethod]
        [DataRow("35%", ".35", 2)]
        [DataRow("35.34%", ".3534", 4)]
        [DataRow("35.123%", ".35123", 5)]
        [DataRow("0.35%", ".0035", 4)]
        [DataRow("35%", ".35", 2)]
        public void GetWriteData_CanCovertDecimalToPercentage(string expectedResult, string inputDataString,
           int numberOfDecimalPlaces)
        {
            // Arrange
            decimal inputData = decimal.Parse(inputDataString);

            var attribute = new CsvConverterNumberAttribute();
            attribute.NumberOfDecimalPlaces = numberOfDecimalPlaces;

            var classUnderTest = new CsvConverterPercentage();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actual = classUnderTest.GetWriteData(typeof(decimal), inputData, ColumName, ColumnIndex, RowNumber);

            // Windows 7 and Windows 10 format strings differently so remove the space so that for the test it doesn't matter
            if (actual != null)
                actual = actual.Replace(" ", "");


            // Assert
            Assert.AreEqual(expectedResult, actual);
        }
    }
}
