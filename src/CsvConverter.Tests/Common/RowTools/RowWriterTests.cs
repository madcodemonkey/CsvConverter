using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.RowTools
{
    [TestClass]
    public class RowWriterTests
    {
        [DataTestMethod]
        [DataRow("Column1", "Column2", "Column3")]
        [DataRow("Ja,ck", "Rabbit\"Data", "John")]
        [DataRow("Jack", "Rabbit", "John\r\nTest")] //  RFC 4180
        public void RowWriterToRowReaderTest(string col1, string col2, string col3)
        {
            // Arrange
            var inputData = new List<string>() { col1, col2, col3 };
            List<string> actualData;

            // Act
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, 512, true))
                {
                    var cut = new RowWriter(sw);

                    cut.Write(inputData);
                }

                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms, Encoding.UTF8, true, 512, true))
                {
                    var rowReader = new RowReader(sr);
                    actualData = rowReader.ReadRow();
                }
            }

            // Assert
            Assert.IsNotNull(actualData, "Row reader didn't do anything.");
            Assert.AreEqual(inputData.Count, actualData.Count, "column count is different");
            for (int i = 0; i < inputData.Count; i++)
            {
                Assert.AreEqual(inputData[i], actualData[i]);
            }
        }


        /// <summary>To adhere to RFC 4180, we need to be able to handle text that has a carriage return and line feed with it.
        /// If there is both a carriage return and line feed (CRLF) together, we need to write that out to the file.</summary>
        [TestMethod]
        public void CRLFInQuotesIsWrittenOutOverMultipleLines()
        {
            // Arrange
            var expectedData = new List<string>() { "Jack", "Rabbit", "John\r\nTest" };

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, 512, true))
                {
                    var cut = new RowWriter(sw);

                    // Act
                    // Act
                    // Act
                    cut.Write(expectedData);
                }

                // Assert
                // Assert
                // Assert
                // Make sure that it was WRITTEN out over two lines
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms, Encoding.UTF8, true, 512, true))
                {
                    // 1st line
                    string actualLine = sr.ReadLine();
                    Assert.AreEqual("Jack,Rabbit,\"John", actualLine);

                    // 2nd line
                    actualLine = sr.ReadLine();
                    Assert.AreEqual("Test\"", actualLine);
                }
            }
        }
    }
}
