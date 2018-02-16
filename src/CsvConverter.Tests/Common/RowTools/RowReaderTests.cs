using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Readers
{
    [TestClass]
    public class RowReaderTests
    {
        [TestMethod]
        public void CanReadHeaderRow()
        {
            using (var ms = GetHeaderRow())
            using (var sr = new StreamReader(ms))
            {
                // Arrange
                var classUnderTest = new RowReader(sr);

                // Act
                List<string> columns = classUnderTest.ReadRow();


                // Assert
                Assert.AreEqual(15, columns.Count, "Expecting 15 columns");
            }
        }

        [TestMethod]
        public void CanReadQoutes()
        {
            using (var ms = GetRowWithQuotes())
            using (var sr = new StreamReader(ms))
            {
                // Arrange
                var classUnderTest = new RowReader(sr);

                // Act
                List<string> columns = classUnderTest.ReadRow();


                // Assert
                Assert.AreEqual(3, columns.Count, "Expecting 3 columns");
                Assert.IsFalse(classUnderTest.IsRowBlank, "Incorrectly identify a blank row");
                Assert.AreEqual("no quotes", columns[0]);
                Assert.AreEqual("\"two quotes\"", columns[1]);
                Assert.AreEqual("One \" quote", columns[2]);
            }
        }

        [TestMethod]
        public void CanIdentifyBlankRow()
        {
            using (var ms = GetBlankRow())
            using (var sr = new StreamReader(ms))
            {
                // Arrange
                var classUnderTest = new RowReader(sr);

                // Act
                List<string> columns = classUnderTest.ReadRow();


                // Assert
                Assert.IsTrue(classUnderTest.IsRowBlank, "Unable to identify a blank row");
            }
        }


        /// <summary>According to RFC 4180 (formatting CSV files)- https://www.ietf.org/rfc/rfc4180.txt
        /// You should be able to read over multiple rows if a carrage return is encounted within 
        /// quotes.</summary>
        [TestMethod]
        public void CanReadOneRecordsOverMultipleLines()
        {
            using (var ms = OneRecordOverMultipleRows())
            using (var sr = new StreamReader(ms))
            {
                // Arrange
                var classUnderTest = new RowReader(sr);

                // Act
                List<string> columns = classUnderTest.ReadRow();
              

                // Assert
                Assert.AreEqual(7, columns.Count, "Expecting 4 columns");
                Assert.IsFalse(classUnderTest.IsRowBlank, "Incorrectly identify a blank row");
                Assert.AreEqual("Head1", columns[0]);
                Assert.AreEqual("Head2", columns[1]);
                Assert.AreEqual("Head3", columns[2]);
                Assert.AreEqual("Head4\r\nTwo-hello-i-want-this-longer", columns[3]);
                Assert.AreEqual("Head5", columns[4]);
                Assert.AreEqual("Head6", columns[5]);
                Assert.AreEqual("Head7", columns[6]);

            }            
        }

        [TestMethod]
        public void CanReadMultipleRecords()
        {
            using (var ms = MultipleRecords())
            using (var sr = new StreamReader(ms))
            {
                // Arrange
                var classUnderTest = new RowReader(sr);

                // Act
                List<string> columns1 = classUnderTest.ReadRow();
                List<string> columns2 = classUnderTest.ReadRow();


                // Assert
                Assert.AreEqual(4, columns1.Count, "Expecting 4 columns");
                Assert.IsFalse(classUnderTest.IsRowBlank, "Incorrectly identify a blank row");
                Assert.AreEqual("Head1", columns1[0]);
                Assert.AreEqual("Head2", columns1[1]);
                Assert.AreEqual("Head3", columns1[2]);
                Assert.AreEqual("Head4", columns1[3]);

                Assert.AreEqual(4, columns2.Count, "Expecting 4 columns");
                Assert.IsFalse(classUnderTest.IsRowBlank, "Incorrectly identify a blank row");
                Assert.AreEqual("Jack1", columns2[0]);
                Assert.AreEqual("Jack2", columns2[1]);
                Assert.AreEqual("Jack3", columns2[2]);
                Assert.AreEqual("Jack4", columns2[3]);

            }
        }
               

        private MemoryStream GetHeaderRow()
        {
            var result = new MemoryStream();
                        
            using (StreamWriter sw = new StreamWriter(result, Encoding.UTF8, 512, true))
            {
                sw.WriteLine("GEO_NAME,GEO_ID,LATITUDE,LONGITUDE,GEOGRAPHYTYPE,PROVINCE,CMACA_ID,CMACA_NAME,COUNTYFIPS,COUNTYNAME,TIMEZONE,Status,,,");
                sw.Flush();
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        private MemoryStream GetRowWithQuotes()
        {
            var result = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(result, Encoding.UTF8, 512, true))
            {
                sw.WriteLine("no quotes,\"\"\"two quotes\"\"\",\"One \"\" quote\"");
                sw.Flush();
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        private MemoryStream GetBlankRow()
        {
            var result = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(result, Encoding.UTF8, 512, true))
            {
                sw.WriteLine(",,,");
                sw.Flush();
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        private MemoryStream MultipleRecords()
        {
            var result = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(result, Encoding.UTF8, 512, true))
            {
                sw.WriteLine("Head1,Head2,Head3,Head4");
                sw.WriteLine("Jack1,Jack2,Jack3,Jack4");
                sw.Flush();
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }


        private MemoryStream OneRecordOverMultipleRows()
        {
            var result = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(result, Encoding.UTF8, 512, true))
            {
                // The CRLF text must be in quotes too!
                sw.WriteLine("Head1,Head2,Head3,\"Head4");
                sw.WriteLine("Two-hello-i-want-this-longer\",Head5,Head6,Head7");
                sw.Flush();
            }

            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

    }
}
