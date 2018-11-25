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
        [DataRow("Hey, diddle, diddle,", "c", "b", true, "Hey, diddle, diddle,")] // no changes
        [DataRow("The cat and the fiddle,", "T", "G", true, "Ghe cat and the fiddle,")]  // target captial letter
        [DataRow("The cow jumped over the moon;", " ", "", true, "Thecowjumpedoverthemoon;")] // Remove spaces
        [DataRow("The little dog laughed", "tt", "ss", true, "The lissle dog laughed")] // captial T untouched
        [DataRow("To see such sport,", "e", "ss", true, "To sssss such sport,")]
        [DataRow("And the dish ran away with the spoon.", "A", "a", true, "and the dish ran away with the spoon.")]
        [DataRow("cat", "c", "b", true, "bat")]
        [DataRow("", "c", "b", true, "")]
        [DataRow(" ", "c", "b", true, " ")]
        [DataRow("0", "0", "", true, "")]
        [DataRow("0", "0 ", "", true, "0")]
        [DataRow(null, null, "hello", true, "hello")]
        [DataRow("", "", "hello2", true, "hello2")]
        [DataRow("Michael's", "'", "", true, "Michaels")]
        [DataRow("Michael\"s", "\"", "", true, "Michaels")]
        [DataRow(null, "a", "", true, null)]
        [DataRow("", "a", "", true, "")]
        [DataRow("Michael", "ae", "", true, "Michl")]
        [DataRow("Michael", "", "", true, "Michael")]
        [DataRow("Michael", null, "", true, "Michael")]
        [DataRow("Michael's", "'", "-", true, "Michael-s")]
        [DataRow("Michael", "chael", "ke", true, "Mike")]
        [DataRow("And the dog and the cat", "and", "or", false, "or the dog or the cat")]
        [DataRow("schaAl", "a", "o", false, "school")]
        public void GetWriteData_CanReplaceText_DataReplaced(string csvField, string oldValue,
            string newValue, bool isCaseSensitive, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(
                typeof(CsvConverterStringReplaceTextEveryMatch))
                { OldValue = oldValue, NewValue = newValue, IsCaseSensitive = isCaseSensitive };

            var classUnderTest = new CsvConverterStringReplaceTextEveryMatch();
            classUnderTest.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            string actualResult = classUnderTest.GetWriteData(typeof(string), csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [DataTestMethod]
        [DataRow("Hey, diddle, diddle,", "c", "b", true, "Hey, diddle, diddle,")] // no changes
        [DataRow("The cat and the fiddle,", "T", "G", true, "Ghe cat and the fiddle,")]  // target captial letter
        [DataRow("The cow jumped over the moon;", " ", "", true, "Thecowjumpedoverthemoon;")] // Remove spaces
        [DataRow("The little dog laughed", "tt", "ss", true, "The lissle dog laughed")] // captial T untouched
        [DataRow("To see such sport,", "e", "ss", true, "To sssss such sport,")]
        [DataRow("And the dish ran away with the spoon.", "A", "a", true, "and the dish ran away with the spoon.")]
        [DataRow("cat", "c", "b", true, "bat")]
        [DataRow("", "c", "b", true, "")]
        [DataRow(" ", "c", "b", true, " ")]
        [DataRow("0", "0", "", true, "")]
        [DataRow("0", "0 ", "", true, "0")]
        [DataRow(null, null, "hello", true, "hello")]
        [DataRow("", "", "hello2", true, "hello2")]
        [DataRow("Michael's", "'", "", true, "Michaels")]
        [DataRow("Michael\"s", "\"", "", true, "Michaels")]
        [DataRow(null, "a", "", true, null)]
        [DataRow("", "a", "", true, "")]
        [DataRow("Michael", "ae", "", true, "Michl")]
        [DataRow("Michael", "", "", true, "Michael")]
        [DataRow("Michael", null, "", true, "Michael")]
        [DataRow("Michael's", "'", "-", true, "Michael-s")]
        [DataRow("Michael", "chael", "ke", true, "Mike")]
        [DataRow("And the dog and the cat", "and", "or", false, "or the dog or the cat")]
        [DataRow("schaAl", "a", "o", false, "school")]
        public void GetReadData_CanReplaceText_DataReplaced(string csvField, string oldValue,
       string newValue, bool isCaseSensitive, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterStringOldAndNewAttribute(
                typeof(CsvConverterStringReplaceTextEveryMatch))
            { OldValue = oldValue, NewValue = newValue, IsCaseSensitive = isCaseSensitive };

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
