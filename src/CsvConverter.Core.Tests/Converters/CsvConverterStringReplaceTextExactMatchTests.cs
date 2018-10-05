using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterStringReplaceTextExactMatchTests
    {
        [DataTestMethod]
        [DataRow("cache", "c", "b", "cache")]  // not a whole word
        [DataRow("cat", "cat", "bat", "bat")]
        [DataRow("", "c", "b", "")]  // no match
        [DataRow(" ", "c", "b", " ")]  // no match
        [DataRow("0", "0", "", "")] // remove 
        [DataRow("dog", "dog", "", "")] // remove 
        [DataRow("dog ", "dog", "", "dog ")] // no match due to trailing space
        [DataRow("0", "0 ", "", "0")]
        public void GetWriteData_CanReplaceText_DataReplaced(string csvField, string oldValue, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceTextExactMatch)) { OldValue = oldValue, NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceTextExactMatch();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualResult = classUnderTest.GetWriteData(typeof(string), csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [DataTestMethod]
        [DataRow("cache", "c", "b", "cache")]  // not a whole word
        [DataRow("cat", "cat", "bat", "bat")]
        [DataRow("", "c", "b", "")]  // no match
        [DataRow(" ", "c", "b", " ")]  // no match
        [DataRow("0", "0", "", "")] // remove 
        [DataRow("dog", "dog", "", "")] // remove 
        [DataRow("dog ", "dog", "", "dog ")] // no match due to trailing space
        [DataRow("0", "0 ", "", "0")]
        public void GetReadData_CanReplaceText_DataReplaced(string csvField, string oldValue, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceTextExactMatch)) { OldValue = oldValue, NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceTextExactMatch();
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
            var attribute = new CsvConverterBooleanAttribute(typeof(CsvConverterStringReplaceTextExactMatch));
            var classUnderTest = new CsvConverterStringReplaceTextExactMatch();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Assert
            Assert.Fail("Should have received an exception for passing in wrong type of attribute.");
        }
    }
}
