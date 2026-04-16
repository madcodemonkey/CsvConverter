namespace CsvConverter.Core.Tests.Converters;

[TestClass]
public class CsvConverterDefaultGuidTests
{
    [TestMethod]
    [DataRow("f44ed1ef-63ba-4aed-b106-14c415bebaa9", "f44ed1ef-63ba-4aed-b106-14c415bebaa9")]
    [DataRow("F44ed1EF-63BA-4AED-B106-14C415BEBAA9", "f44ed1ef-63ba-4aed-b106-14c415bebaa9")] // Upper case letters
    [DataRow("f44ed1ef63ba4aedb10614c415bebaa9", "f44ed1ef-63ba-4aed-b106-14c415bebaa9")]    // N format
    [DataRow("F44ed1EF63BA4AEDB10614C415BEBAA9", "f44ed1ef-63ba-4aed-b106-14c415bebaa9")] // N format with Upper case letters
    public void GetReadData_CanConvertNonNullableGuidsWithoutAnAttribute_ValuesConverted(string inputData, string expected)
    {
        // Arrange
        Guid expectedGuid = Guid.Parse(expected);
        var cut = new CsvConverterDefaultGuid();
        cut.Initialize(null, new DefaultTypeConverterFactory());

        // Act
        Guid actual = (Guid)cut.GetReadData(typeof(int), inputData, "Column1", 1, 1);

        // Assert
        Assert.AreEqual(expectedGuid, actual);
    }

    [TestMethod]
    [DataRow("f44ed1ef-63ba-4aed-b106-14c415bebaa9", "f44ed1ef-63ba-4aed-b106-14c415bebaa9")]
    [DataRow("", null)]
    [DataRow(null, null)]
    public void GetReadData_CanConvertNullableGuidsWithoutAnAttribute_ValuesConverted(string inputData, string expected)
    {
        // Arrange
        Guid? expectedGuid = null;
        if (Guid.TryParse(inputData, out var someGuid))
        {
            expectedGuid = someGuid;
        }

        var cut = new CsvConverterDefaultGuid();
        cut.Initialize(null, new DefaultTypeConverterFactory());

        // Act
        Guid? actual = (Guid?)cut.GetReadData(typeof(Guid?), inputData, "Column1", 1, 1);

        // Assert
        Assert.AreEqual(expectedGuid, actual);
    }

    [TestMethod]
    [DataRow("abc")]
    [DataRow("5488e4$#@#")]
    public void GetReadData_CannotHandleNonGuidStrings_ThrowsException(string inputData)
    {
        // Arrange
        var cut = new CsvConverterDefaultGuid();
        cut.Initialize(null, new DefaultTypeConverterFactory());

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var actual = (Guid)cut.GetReadData(typeof(Guid), inputData, "Column1", 1, 1);
        });
    }
}
