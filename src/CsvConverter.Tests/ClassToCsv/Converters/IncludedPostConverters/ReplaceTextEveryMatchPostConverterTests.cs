using System;
using CsvConverter;
using CsvConverter.ClassToCsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassToCsv.Converters
{
    [TestClass]
    public class ReplaceTextEveryMatchPostConverterTests
    {
        [DataTestMethod]
        [DataRow("cat", "c", "b", "bat")]
        [DataRow("", "c", "b", "")]
        [DataRow(" ", "c", "b", " ")]
        [DataRow("0", "0", "", "")]
        [DataRow("0", "0 ", "", "0")]
        public void Convert_CanReplaceText_DataReplaced(string csvField, string oldValue, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new CsvConverterOldAndNewValueAttribute(typeof(ReplaceTextEveryMatchClassToCsvPostConverter)) { OldValue = oldValue, NewValue = newValue };

            var classUnderTest = new ReplaceTextEveryMatchClassToCsvPostConverter();
            classUnderTest.Initialize(attribute);

            // Act
            string actualResult = classUnderTest.Convert(csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Initialize_PassingInTheWrongTypeOfPostConverter_ResultsInException()
        {
            // Arrange 
            var attribute = new CsvConverterCustomAttribute(typeof(ReplaceTextEveryMatchClassToCsvPostConverter));
            var classUnderTest = new ReplaceTextEveryMatchClassToCsvPostConverter();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in wrong type of attribute.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Initialize_SpecifyingAnOldValueOfNullIsNotAllowed_ResultsInException()
        {
            // Arrange 
            var attribute = new CsvConverterOldAndNewValueAttribute(typeof(ReplaceTextEveryMatchClassToCsvPostConverter)) { OldValue = null, NewValue = "Some new value" };
            var classUnderTest = new ReplaceTextEveryMatchClassToCsvPostConverter();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in a bad old value.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Initialize_SpecifyingAnOldValueOfZeroLengthIsNotAllowed_ResultsInException()
        {
            // Arrange 
            var attribute = new CsvConverterOldAndNewValueAttribute(typeof(ReplaceTextEveryMatchClassToCsvPostConverter)) { OldValue = "", NewValue = "Some new value" };
            var classUnderTest = new ReplaceTextEveryMatchClassToCsvPostConverter();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in a bad old value.");
        }
    }
}
