using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterStringReplaceTextEveryMatchTests
    {
        [DataTestMethod]
        [DataRow("Hey, diddle, diddle,", "c", "b", "Hey, diddle, diddle,")] // no changes
        [DataRow("The cat and the fiddle,", "T", "G", "Ghe cat and the fiddle,")]  // target captial letter
        [DataRow("The cow jumped over the moon;", " ", "", "Thecowjumpedoverthemoon;")] // Remove spaces
        [DataRow("The little dog laughed", "tt", "ss", "The lissle dog laughed")] // captial T untouched
        [DataRow("To see such sport,", "e", "ss", "To sssss such sport,")]
        [DataRow("And the dish ran away with the spoon.", "A", "a", "and the dish ran away with the spoon.")]
        [DataRow("cat", "c", "b", "bat")]
        [DataRow("", "c", "b", "")]
        [DataRow(" ", "c", "b", " ")]
        [DataRow("0", "0", "", "")]
        [DataRow("0", "0 ", "", "0")]
        [DataRow(null, null, "hello", "hello")]
        [DataRow("", "", "hello2", "hello2")]
        [DataRow("Michael's", "'", "", "Michaels")]
        [DataRow("Michael\"s", "\"", "", "Michaels")]
        [DataRow(null, "a", "", null)]
        [DataRow("", "a", "", "")]
        [DataRow("Michael", "ae", "", "Michl")]
        [DataRow("Michael", "", "", "Michael")]
        [DataRow("Michael", null, "", "Michael")]
        [DataRow("Michael's", "'", "-", "Michael-s")]
        [DataRow("Michael", "chael", "ke", "Mike")]
        public void GetWriteData_CanReplaceText_DataReplaced(string csvField, string oldValue,
            string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceTextEveryMatch)) { OldValue = oldValue, NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceTextEveryMatch();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualResult = classUnderTest.GetWriteData(typeof(string), csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [DataTestMethod]
        [DataRow("Hey, diddle, diddle,", "c", "b", "Hey, diddle, diddle,")] // no changes
        [DataRow("The cat and the fiddle,", "T", "G", "Ghe cat and the fiddle,")]  // target captial letter
        [DataRow("The cow jumped over the moon;", " ", "", "Thecowjumpedoverthemoon;")] // Remove spaces
        [DataRow("The little dog laughed", "tt", "ss", "The lissle dog laughed")] // captial T untouched
        [DataRow("To see such sport,", "e", "ss", "To sssss such sport,")]
        [DataRow("And the dish ran away with the spoon.", "A", "a", "and the dish ran away with the spoon.")]
        [DataRow("cat", "c", "b", "bat")]
        [DataRow("", "c", "b", "")]
        [DataRow(" ", "c", "b", " ")]
        [DataRow("0", "0", "", "")]
        [DataRow("0", "0 ", "", "0")]
        [DataRow(null, null, "hello", "hello")]
        [DataRow("", "", "hello2", "hello2")]
        [DataRow("Michael's", "'", "", "Michaels")]
        [DataRow("Michael\"s", "\"", "", "Michaels")]
        [DataRow(null, "a", "", null)]
        [DataRow("", "a", "", "")]
        [DataRow("Michael", "ae", "", "Michl")]
        [DataRow("Michael", "", "", "Michael")]
        [DataRow("Michael", null, "", "Michael")]
        [DataRow("Michael's", "'", "-", "Michael-s")]
        [DataRow("Michael", "chael", "ke", "Mike")]
        public void GetReadData_CanReplaceText_DataReplaced(string csvField, string oldValue,
       string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(typeof(CsvConverterStringReplaceTextEveryMatch)) { OldValue = oldValue, NewValue = newValue };

            var classUnderTest = new CsvConverterStringReplaceTextEveryMatch();
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
            var attribute = new CsvConverterBooleanAttribute(typeof(CsvConverterStringReplaceTextEveryMatch));
            var classUnderTest = new CsvConverterStringReplaceTextEveryMatch();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Assert
            Assert.Fail("Should have received an exception for passing in wrong type of attribute.");
        }
    }

}
