using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterStringReplaceNullOrWhiteSpaceWithNewValueTests
    {
        [TestMethod]
        [DataRow("cache", "c", "cache")]
        [DataRow("cat", "cat", "cat")]
        [DataRow("", "c", "c")]
        [DataRow(" ", "c", "c")]
        [DataRow("    ", "c", "c")]
        [DataRow(null, "c", "c")]
        [DataRow("0", "0", "0")]
        [DataRow("dog", "dog", "dog")]
        [DataRow("dog ", "dog", "dog ")]
        [DataRow("0", "0 ", "0")]
        public void GetWriteData_CanReplaceText_DataReplaced(string csvField, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue)) { NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualResult = classUnderTest.GetWriteData(typeof(string), csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [DataRow("cache", "c", "cache")]
        [DataRow("cat", "cat", "cat")]
        [DataRow("", "c", "c")]
        [DataRow(" ", "c", "c")]
        [DataRow("    ", "c", "c")]
        [DataRow(null, "c", "c")]
        [DataRow("0", "0", "0")]
        [DataRow("dog", "dog", "dog")]
        [DataRow("dog ", "dog", "dog ")]
        [DataRow("0", "0 ", "0")]
        public void GetReadData_CanReplaceText_DataReplaced(string csvField, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue)) { NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            object actualResult = classUnderTest.GetReadData(typeof(string), csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Initialize_PassingInTheWrongTypeOfAttributeToThePostConverter_ResultsInException()
        {
            // Arrange 
            var attribute = new CsvConverterBooleanAttribute(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue));
            var classUnderTest = new CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue();

            // Act & Assert
            Assert.Throws<CsvConverterAttributeException>(() =>
            {
                classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());
            });
        }
    }
}
