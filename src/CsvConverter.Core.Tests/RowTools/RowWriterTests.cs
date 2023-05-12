using System.Collections.Generic;
using System.IO;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests;

[TestClass]
public class RowWriterTests
{
    [DataTestMethod]
    [DataRow("Finish with new laptop setup.  Go to UPS store and return laptop", "Finish with new laptop setup.  Go to UPS store and return laptop", "Normal string should work properly")]
    [DataRow("Finish with new laptop setup, Go to UPS store and return laptop", "\"Finish with new laptop setup, Go to UPS store and return laptop\"", "Split character ',' in the string should be escaped.")]
    [DataRow("Finish with new laptop setup.\nGo to UPS store and return laptop", "\"Finish with new laptop setup.\nGo to UPS store and return laptop\"", "New line in the string should be escaped.")]
    [DataRow("Finish with new laptop setup.\rGo to UPS store and return laptop", "\"Finish with new laptop setup.\rGo to UPS store and return laptop\"", "Carriage return in the string should be escaped.")]
    [DataRow("Finish with new laptop setup.\r\nGo to UPS store and return laptop", "\"Finish with new laptop setup.\r\nGo to UPS store and return laptop\"", "Carriage return and new line in the string should be escaped.")]
    public void Write_CanCorrectlyWriteTextToStream(string input, string expectedOutput, string message)
    {
        // Arrange
        using MemoryStream outputStream = new MemoryStream();
        using StreamWriter outputWriter = new StreamWriter(outputStream, leaveOpen: true);
        outputWriter.WriteLine(expectedOutput);
        outputWriter.Flush();
        outputWriter.Close();

        outputStream.Position = 0;
        byte[] expectedData = outputStream.ToArray();
       

        using MemoryStream inputStream = new MemoryStream();
        using StreamWriter inputWriter = new StreamWriter(inputStream, leaveOpen: true);
        
        RowWriter rw = new RowWriter(inputWriter);

        // Act
        rw.Write(new List<string> { input });
        inputWriter.Flush();
        inputWriter.Close();

        // Assert
        inputStream.Position = 0;
        byte[] actualData = inputStream.ToArray();
        Assert.AreEqual(expectedData.Length, actualData.Length, message);

        for (var index = 0; index < actualData.Length; index++)
        {
            var actualItem = actualData[index];
            var expectedItem = expectedData[index];
            Assert.AreEqual(expectedItem, actualItem, $"Failure at index {index}.  {message}");
        } 
    }
}