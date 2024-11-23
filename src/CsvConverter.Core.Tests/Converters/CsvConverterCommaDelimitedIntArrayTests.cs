namespace CsvConverter.Core.Tests.Converters;

[TestClass]
public class CsvConverterCommaDelimitedIntArrayTests
{
    // Default is AllowRounding = false so that we don't lose precision by default!
    [DataTestMethod]
    [DataRow("1,2,3,4", "1,2,3,4")]
    [DataRow("1 ,2 ,3 , 4 ", "1,2,3,4")]
    public void GetReadData_CanConvertStringToIntArray_ValuesConverted(string inputData, string expected)
    {
        // Arrange
        int[] expectedArray = expected.Split(',').Select(int.Parse).ToArray();

        var attribute = new CsvConverterNumberAttribute();
        var cut = new CsvConverterCommaDelimitedIntArray();
        cut.Initialize(attribute, new DefaultTypeConverterFactory());

        // Act
        int[] actual = (int[])cut.GetReadData(typeof(int[]), inputData, "Column1", 1, 1);

        // Assert
        Assert.AreEqual(expectedArray.Length, actual.Length);
        for (int i = 0; i < expectedArray.Length; i++)
        {
            Assert.AreEqual(expectedArray[i], actual[i]);
        }
    }

    [DataTestMethod]
    [DataRow("1,b,3,4")] // letter and not number
    [DataRow("1,2,,4")]  // empty string
    public void GetReadData_BadData_ReturnsException(string inputData)
    {
        // Arrange
        var attribute = new CsvConverterNumberAttribute();
        var cut = new CsvConverterCommaDelimitedIntArray();
        cut.Initialize(attribute, new DefaultTypeConverterFactory());

        // Act
        try
        {
            cut.GetReadData(typeof(int[]), inputData, "Column1", 1, 1);

            Assert.Fail("Expected exception!");
        }
        catch (ArgumentException ex)
        {
            // Assert
            Assert.IsTrue(ex.Message.IndexOf("value at index", StringComparison.Ordinal) > 0);
        }
    }

    [TestMethod]
    public void GetReadData_CanHandleEmptyString_ReturnsEmptyArray()
    {
        // Arrange
        var attribute = new CsvConverterNumberAttribute();
        var cut = new CsvConverterCommaDelimitedIntArray();
        cut.Initialize(attribute, new DefaultTypeConverterFactory());

        // Act
        int[] actual = (int[])cut.GetReadData(typeof(int[]), "", "Column1", 1, 1);

        // Assert
        Assert.IsTrue(actual.Length == 0);
    }

    [TestMethod]
    public void GetWriteData_CanWriteString_WhenGivenAnEmptyArray()
    {
        // Arrange
        var attribute = new CsvConverterNumberAttribute();
        var cut = new CsvConverterCommaDelimitedIntArray();
        cut.Initialize(attribute, new DefaultTypeConverterFactory());

        // Act
        var actual = cut.GetWriteData(typeof(int[]), Array.Empty<int>(), "Column1", 1, 1);

        // Assert
        Assert.AreEqual(string.Empty, actual);
    }

    [TestMethod]
    public void GetWriteData_CanWriteString_WhenGivenAnArrayOfData()
    {
        // Arrange
        var attribute = new CsvConverterNumberAttribute();
        var cut = new CsvConverterCommaDelimitedIntArray();
        cut.Initialize(attribute, new DefaultTypeConverterFactory());

        // Act
        var actual = cut.GetWriteData(typeof(int[]), new[] { 1, 2, 3, 4 }, "Column1", 1, 1);

        // Assert
        Assert.AreEqual("1,2,3,4", actual);
    }

}