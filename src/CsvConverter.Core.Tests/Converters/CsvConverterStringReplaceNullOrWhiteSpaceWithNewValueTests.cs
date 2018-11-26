using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterStringReplaceNullOrWhiteSpaceWithNewValueTests
    {
        [DataTestMethod]
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

        [DataTestMethod]
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
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void Initialize_PassingInTheWrongTypeOfAttributeToThePostConverter_ResultsInException()
        {
            // Arrange 
            var attribute = new CsvConverterBooleanAttribute(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue));
            var classUnderTest = new CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Assert
            Assert.Fail("Should have received an exception for passing in wrong type of attribute.");
        }
    }
}
