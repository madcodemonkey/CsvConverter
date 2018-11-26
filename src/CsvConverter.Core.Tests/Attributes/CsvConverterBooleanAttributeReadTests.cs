using System;
using System.Collections.Generic;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterBooleanAttributeReadTests
    {
        [TestMethod]
        public void GetRecord_ConvertBooleansOfVariousFormatsWithoutAnAttribute_ValuesConverted()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Bool1", "Bool2", "Bool3", "Bool4", "Bool5" })
                .Returns(new List<string> { "1", "Yes", "Y", "True", "T", "1" })
                .Returns(new List<string> { "2", "No", "N", "False", "F", "0" });

            var classUnderTest = new CsvReaderService<CsvConverterBooleanAttributeReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterBooleanAttributeReadData1 row1 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData1 row2 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData1 row3 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(true, row1.Bool1);
            Assert.AreEqual(true, row1.Bool2);
            Assert.AreEqual(true, row1.Bool3);
            Assert.AreEqual(true, row1.Bool4);
            Assert.AreEqual(true, row1.Bool5);

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual(false, row2.Bool1);
            Assert.AreEqual(false, row2.Bool2);
            Assert.AreEqual(false, row2.Bool3);
            Assert.AreEqual(false, row2.Bool4);
            Assert.AreEqual(false, row2.Bool5);

            Assert.IsNull(row3, "There is no 3rd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", "Dog", true, "Frog", true)]
        [DataRow("1", "Cat", false, "Pond", false)]
        public void GetRecord_CanConvertCustomTextToBooleanWhenSpecifiedOnProperty_ValuesConverted(string order,
            string catsOrDogsJumpInput, bool catsOrDogsJumpExpected,
            string frogsJumpInput, bool frogsJumpExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "CatsOrDogsJump", "FrogsJump" })
                .Returns(new List<string> { order, catsOrDogsJumpInput, frogsJumpInput });

            var classUnderTest = new CsvReaderService<CsvConverterBooleanAttributeReadData3>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterBooleanAttributeReadData3 row1 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(catsOrDogsJumpExpected, row1.CatsOrDogsJump);
            Assert.AreEqual(frogsJumpExpected, row1.FrogsJump);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", "Dog", true, "Cat", false)]
        [DataRow("1", "Cat", false, "Dog", true)]
        public void GetRecord_WhenClassAttributeIsUsedBooleansAreConvertedTheSameWay_ValuesConverted(string order,
            string catsOrDogsJumpInput, bool catsOrDogsJumpExpected,
            string frogsJumpInput, bool frogsJumpExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "CatsOrDogsJump", "FrogsJump" })
                .Returns(new List<string> { order, catsOrDogsJumpInput, frogsJumpInput });

            var classUnderTest = new CsvReaderService<CsvConverterBooleanAttributeReadData4>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterBooleanAttributeReadData4 row1 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData4 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(catsOrDogsJumpExpected, row1.CatsOrDogsJump);
            Assert.AreEqual(frogsJumpExpected, row1.FrogsJump);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", "Dog", true, "Frog", true)]
        [DataRow("1", "Cat", false, "Pond", false)]
        public void GetRecord_WhenClassAttributeIsUsedYouCanOverrideIndividualProperties_ValuesConverted(string order,
             string catsOrDogsJumpInput, bool catsOrDogsJumpExpected,
             string frogsJumpInput, bool frogsJumpExpected)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "CatsOrDogsJump", "FrogsJump" })
                .Returns(new List<string> { order, catsOrDogsJumpInput, frogsJumpInput });

            var classUnderTest = new CsvReaderService<CsvConverterBooleanAttributeReadData5>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterBooleanAttributeReadData5 row1 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData5 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual(catsOrDogsJumpExpected, row1.CatsOrDogsJump);
            Assert.AreEqual(frogsJumpExpected, row1.FrogsJump);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("1", "Dog")]
        [DataRow("1", "Cat")]
        [ExpectedException(typeof(CsvConverterException))]
        public void GetRecord_ThrowsExceptionsForBogusData_ExceptionThrown(string order, string canJump)
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "CanJump" })
                .Returns(new List<string> { order, canJump });

            var classUnderTest = new CsvReaderService<CsvConverterBooleanAttributeReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterBooleanAttributeReadData2 row1 = classUnderTest.GetRecord();
            CsvConverterBooleanAttributeReadData2 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should recieve an exception above for bogus input data");
        }

    }

    internal class CsvConverterBooleanAttributeReadData1
    {
        public int Order { get; set; }

        public bool Bool1 { get; set; }

        public bool Bool2 { get; set; }

        public bool Bool3 { get; set; }

        public bool Bool4 { get; set; }

        public bool Bool5 { get; set; }
    }

    internal class CsvConverterBooleanAttributeReadData2
    {
        public int Order { get; set; }

        public bool CanJump { get; set; }
    }

    internal class CsvConverterBooleanAttributeReadData3
    {
        public int Order { get; set; }

        [CsvConverterBoolean(TrueValue = "Dog", FalseValue = "Cat")]
        public bool CatsOrDogsJump { get; set; }

        [CsvConverterBoolean(TrueValue = "Frog", FalseValue = "Pond")]
        public bool FrogsJump { get; set; }
    }

    [CsvConverterBoolean(TrueValue = "Dog", FalseValue = "Cat", TargetPropertyType = typeof(bool))]
    [CsvConverterBoolean(TrueValue = "Dog", FalseValue = "Cat", TargetPropertyType = typeof(bool?))]
    internal class CsvConverterBooleanAttributeReadData4
    {
        public int Order { get; set; }

        public bool CatsOrDogsJump { get; set; }

        public bool? FrogsJump { get; set; }
    }

    [CsvConverterBoolean(TrueValue = "Dog", FalseValue = "Cat", TargetPropertyType = typeof(bool))]
    [CsvConverterBoolean(TrueValue = "Dog", FalseValue = "Cat", TargetPropertyType = typeof(bool?))]
    internal class CsvConverterBooleanAttributeReadData5
    {
        public int Order { get; set; }

        public bool CatsOrDogsJump { get; set; }

        [CsvConverterBoolean(TrueValue = "Frog", FalseValue = "Pond")]
        public bool? FrogsJump { get; set; }
    }
}
