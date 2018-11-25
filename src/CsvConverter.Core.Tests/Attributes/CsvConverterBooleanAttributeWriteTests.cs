using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterBooleanAttributeWriteTests
    {
        [DataTestMethod]
        [DataRow(true, "1", true, "T", true, "True", true, "Yes", true, "Y")]
        [DataRow(false, "0", false, "F", false, "False", false, "No", false, "N")]
        [DataRow(true, "1", true, "T", null, "", true, "Yes", true, "Y")]
        public void Convert_WhenSpecifiedOnEachProperty_BooleansAreConvertedAccordingToAttributes(
            bool bool1Input, string bool1ExpectedOutput,
            bool bool2Input, string bool2ExpectedOutput,
            bool? bool3Input, string bool3ExpectedOutput,
            bool bool4Input, string bool4ExpectedOutput,
            bool bool5Input, string bool5ExpectedOutput)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterBooleanWriteData1>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterBooleanWriteData1() { Bool1 = bool1Input, Bool2 = bool2Input, Bool3 = bool3Input, Bool4 = bool4Input, Bool5 = bool5Input };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            Assert.AreEqual(bool1ExpectedOutput, dataRow[0]);
            Assert.AreEqual(bool2ExpectedOutput, dataRow[1]);
            Assert.AreEqual(bool3ExpectedOutput, dataRow[2]);
            Assert.AreEqual(bool4ExpectedOutput, dataRow[3]);
            Assert.AreEqual(bool5ExpectedOutput, dataRow[4]);
        }

        [DataTestMethod]
        [DataRow(true, "Yes", true, "Yes", true, "Yes", true, "Yes", true, "Yes")]
        [DataRow(false, "No", false, "No", false, "No", false, "No", false, "No")]
        [DataRow(false, "No", false, "No", null, "", false, "No", false, "No")]
        public void Convert_WhenSpecifiedOnTheClass_BooleansAreAllConvertedTheSameWay(
            bool bool1Input, string bool1ExpectedOutput,
            bool bool2Input, string bool2ExpectedOutput,
            bool? bool3Input, string bool3ExpectedOutput,
            bool bool4Input, string bool4ExpectedOutput,
            bool bool5Input, string bool5ExpectedOutput)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterBooleanWriteData2>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterBooleanWriteData2() { Bool1 = bool1Input, Bool2 = bool2Input, Bool3 = bool3Input, Bool4 = bool4Input, Bool5 = bool5Input };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            Assert.AreEqual(bool1ExpectedOutput, dataRow[0]);
            Assert.AreEqual(bool2ExpectedOutput, dataRow[1]);
            Assert.AreEqual(bool3ExpectedOutput, dataRow[2]);
            Assert.AreEqual(bool4ExpectedOutput, dataRow[3]);
            Assert.AreEqual(bool5ExpectedOutput, dataRow[4]);
        }


        [DataTestMethod]
        [DataRow(true, "Yes", true, "Yes", true, "Yes", true, "Yes", true, "Y")]
        [DataRow(false, "No", false, "No", false, "No", false, "No", false, "N")]
        [DataRow(false, "No", false, "No", null, "", false, "No", false, "N")]
        public void Convert_WhenSpecifiedTheClassPropertyAttributesCanOverrideClassAttributes_PropertiesTakePrecedence(
            bool bool1Input, string bool1ExpectedOutput,
            bool bool2Input, string bool2ExpectedOutput,
            bool? bool3Input, string bool3ExpectedOutput,
            bool bool4Input, string bool4ExpectedOutput,
            bool bool5Input, string bool5ExpectedOutput)
        {
            // Arrange
            var rowWriterMock = new FakeRowWriter();
            var classUnderTest = new CsvWriterService<CsvConverterBooleanWriteData3>(rowWriterMock);
            classUnderTest.Configuration.HasHeaderRow = true;

            var data = new CsvConverterBooleanWriteData3() { Bool1 = bool1Input, Bool2 = bool2Input, Bool3 = bool3Input, Bool4 = bool4Input, Bool5 = bool5Input };

            // Act
            classUnderTest.WriteRecord(data);

            // Assert
            Assert.AreEqual(2, rowWriterMock.Rows.Count);
            var dataRow = rowWriterMock.Rows[1];

            Assert.AreEqual(bool1ExpectedOutput, dataRow[0]);
            Assert.AreEqual(bool2ExpectedOutput, dataRow[1]);
            Assert.AreEqual(bool3ExpectedOutput, dataRow[2]);
            Assert.AreEqual(bool4ExpectedOutput, dataRow[3]);
            Assert.AreEqual(bool5ExpectedOutput, dataRow[4]);
        }
    }


    internal class CsvConverterBooleanWriteData1
    {
        [CsvConverterBoolean(TrueValue = "1", FalseValue = "0")]
        public bool Bool1 { get; set; }

        [CsvConverterBoolean(TrueValue = "T", FalseValue = "F")]
        public bool Bool2 { get; set; }

        [CsvConverterBoolean(TrueValue = "True", FalseValue = "False")]
        public bool? Bool3 { get; set; }

        [CsvConverterBoolean(TrueValue = "Yes", FalseValue = "No")]
        public bool Bool4 { get; set; }

        [CsvConverterBoolean(TrueValue = "Y", FalseValue = "N")]
        public bool Bool5 { get; set; }
    }

    [CsvConverterBoolean(TrueValue = "Yes", FalseValue = "No", TargetPropertyType = typeof(bool))]
    [CsvConverterBoolean(TrueValue = "Yes", FalseValue = "No", TargetPropertyType = typeof(bool?))]
    internal class CsvConverterBooleanWriteData2
    {
        public bool Bool1 { get; set; }

        public bool Bool2 { get; set; }

        public bool? Bool3 { get; set; }

        public bool Bool4 { get; set; }

        public bool Bool5 { get; set; }
    }


    [CsvConverterBoolean(TrueValue = "Yes", FalseValue = "No", TargetPropertyType = typeof(bool))]
    [CsvConverterBoolean(TrueValue = "Yes", FalseValue = "No", TargetPropertyType = typeof(bool?))]
    internal class CsvConverterBooleanWriteData3
    {
        public bool Bool1 { get; set; }

        public bool Bool2 { get; set; }

        public bool? Bool3 { get; set; }

        public bool Bool4 { get; set; }

        [CsvConverterBoolean(TrueValue = "Y", FalseValue = "N")]
        public bool Bool5 { get; set; }
    }
}
