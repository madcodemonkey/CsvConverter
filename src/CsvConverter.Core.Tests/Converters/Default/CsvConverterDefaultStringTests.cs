using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultStringTests
    {
        [DataTestMethod]
        [DataRow("12345", "12345")]
        [DataRow("Text with spaces", "Text with spaces")]
        [DataRow("", "")]
        [DataRow(null, null)]
        [DataRow("  12345", "  12345")]
        [DataRow("12345  ", "12345  ")]
        public void GetReadData_StringCanAssignValues_ValuesAssigned(string inputData, string expected)
        {
            var cut = new CsvConverterDefaultString();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            string actual = (string)cut.GetReadData(typeof(string), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
