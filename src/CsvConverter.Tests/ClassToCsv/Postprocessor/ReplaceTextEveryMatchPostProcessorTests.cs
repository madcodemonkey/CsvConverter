using System;
using CsvConverter.ClassToCsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassToCsv.Tests.ClassToCsv.Postprocessor
{
    [TestClass]
    public class ReplaceTextEveryMatchPostProcessorTests
    {
        [DataTestMethod]
        [DataRow("cat", "c", "b", "bat")]
        [DataRow("" , "c", "b", "")]
        [DataRow(" ", "c", "b", " ")]
        [DataRow("0", "0", "" , "")]
        [DataRow("0", "0 ", "" , "0")]
        public void CanReplaceText(string csvField, string oldValue, string newValue, string expectedResult)
        {
            // Arrange 
            var attribute = new ClassToCsvReplaceTextPostprocessorAttribute(typeof(ReplaceTextEveryMatchPostProcessor), oldValue, newValue);

            var classUnderTest = new ReplaceTextEveryMatchPostProcessor();
            classUnderTest.Initialize(attribute);

            // Act
            string actualResult = classUnderTest.Work(csvField, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PassingInTheWrongTypeOfPostProcessor_ResultsInException()
        {
            // Arrange 
            var attribute = new ClassToCsvPostProcessorAttribute(typeof(ReplaceTextEveryMatchPostProcessor));
            var classUnderTest = new ReplaceTextEveryMatchPostProcessor();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in wrong type of attribute.");


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpecifyingAnOldValueOfNullIsNotAllowed_ResultsInException()
        {
            // Arrange 
            var attribute = new ClassToCsvReplaceTextPostprocessorAttribute(typeof(ReplaceTextEveryMatchPostProcessor), null, "Some new value");
            var classUnderTest = new ReplaceTextEveryMatchPostProcessor();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in a bad old value.");
        }

        [TestMethod]
         [ExpectedException(typeof(ArgumentException))]
        public void SpecifyingAnOldValueOfZeroLengthIsNotAllowed_ResultsInException()
        {
            // Arrange 
            var attribute = new ClassToCsvReplaceTextPostprocessorAttribute(typeof(ReplaceTextEveryMatchPostProcessor), "", "Some new value");
            var classUnderTest = new ReplaceTextEveryMatchPostProcessor();
            classUnderTest.Initialize(attribute);

            // Assert
            Assert.Fail("Should have received an exception for passing in a bad old value.");
        }
    }

    
}
