using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Converters
{
    [TestClass]
    public class CsvConverterDefaultBooleanTests
    {
        [DataTestMethod]
        [DataRow(true, "1", "0", "1")]
        [DataRow(false, "1", "0", "0")]
        [DataRow(true, "T", "F", "T")]
        [DataRow(false, "T", "F", "F")]
        [DataRow(true, "True", "False", "True")]
        [DataRow(false, "True", "False", "False")]
        [DataRow(true, "Y", "N", "Y")]
        [DataRow(false, "Y", "N", "N")]
        [DataRow(true, "Yes", "No", "Yes")]
        [DataRow(false, "Yes", "No", "No")]
        [DataRow(true, "Frog", "Cat", "Frog")]
        [DataRow(false, "Frog", "Cat", "Cat")]
        public void GetWriteData_CanConvertBoolean_BoolConverted(bool inputValue, string trueValue, string falseValue, string expectedData)
        {
            // Arrange
            var cut = new CsvConverterDefaultBoolean();
            cut.FalseValue = falseValue;
            cut.TrueValue = trueValue;

            // Act
            string actualData = cut.GetWriteData(typeof(bool), inputValue, "Column1",1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(true, "1", "0", "1")]
        [DataRow(false, "1", "0", "0")]
        [DataRow(true, "T", "F", "T")]
        [DataRow(false, "T", "F", "F")]
        [DataRow(true, "True", "False", "True")]
        [DataRow(false, "True", "False", "False")]
        [DataRow(true, "Y", "N", "Y")]
        [DataRow(false, "Y", "N", "N")]
        [DataRow(true, "Yes", "No", "Yes")]
        [DataRow(false, "Yes", "No", "No")]
        [DataRow(true, "Frog", "Cat", "Frog")]
        [DataRow(false, "Frog", "Cat", "Cat")]
        public void GetWriteData_CanConvertNullableBoolean_BoolConverted(bool inputValue, string trueValue, string falseValue, string expectedData)
        {
            // Arrange
            var cut = new CsvConverterDefaultBoolean();
            cut.FalseValue = falseValue;
            cut.TrueValue = trueValue;

            // Act
            string actualData = cut.GetWriteData(typeof(bool?), inputValue, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow("True", true)]
        [DataRow("true", true)]
        [DataRow("TRUE", true)]
        [DataRow("T", true)]
        [DataRow("t", true)]
        [DataRow("Yes", true)]
        [DataRow("YES", true)]
        [DataRow("Y", true)]
        [DataRow("y", true)]
        [DataRow("1", true)]
        [DataRow("False", false)]
        [DataRow("false", false)]
        [DataRow("FALSE", false)]
        [DataRow("f", false)]
        [DataRow("No", false)]
        [DataRow("no", false)]
        [DataRow("NO", false)]
        [DataRow("n", false)]
        [DataRow("0", false)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        public void GetReadData_CanConvertNonNullableBooleansWithoutAnAttribute_ValuesConverted(string inputData, bool expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultBoolean();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            bool actual = (bool)cut.GetReadData(typeof(bool), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("", null)]
        [DataRow("   ", null)]
        [DataRow("True", true)]
        [DataRow("true", true)]
        [DataRow("TRUE", true)]
        [DataRow("T", true)]
        [DataRow("t", true)]
        [DataRow("Yes", true)]
        [DataRow("YES", true)]
        [DataRow("Y", true)]
        [DataRow("y", true)]
        [DataRow("1", true)]
        [DataRow("False", false)]
        [DataRow("false", false)]
        [DataRow("FALSE", false)]
        [DataRow("f", false)]
        [DataRow("No", false)]
        [DataRow("no", false)]
        [DataRow("NO", false)]
        [DataRow("n", false)]
        [DataRow("0", false)]
        public void GetReadData_CanConvertNullableBooleansWithoutAnAttribute_ValuesConverted(string inputData, bool? expected)
        {
            // Arrange
            var cut = new CsvConverterDefaultBoolean();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            bool? actual = (bool?)cut.GetReadData(typeof(bool?), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("5488e4$#@#")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetReadData_CanHandleNonIntegerStrings_ThrowsException(string inputData)
        {
            // Arrange
            var cut = new CsvConverterDefaultBoolean();
            cut.Initialize(null, new DefaultTypeConverterFactory());

            // Act
            bool actual = (bool)cut.GetReadData(typeof(bool), inputData, "Column1", 1, 1);

            // Assert
            Assert.Fail("Exception should be thrown when invalid values are passed into the parser!");
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("Frog", false)]
        [DataRow("frog", false)]
        [DataRow("FROG", false)]
        [DataRow("Cat", true)]
        [DataRow("cat", true)]
        [DataRow("CAT", true)]
        public void GetReadData_CanConvertSpecializedBooleanIfAnAttributeIsProvided_ValuesConverted(string inputData, bool expected)
        {
            // Arrange
            var attribute = new CsvConverterBooleanAttribute();
            attribute.TrueValue = "Cat";
            attribute.FalseValue = "Frog";

            var cut = new CsvConverterDefaultBoolean();
            cut.Initialize(attribute, new DefaultTypeConverterFactory());

            // Act
            bool actual = (bool)cut.GetReadData(typeof(bool), inputData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
