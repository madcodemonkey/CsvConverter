using CsvConverter.RowTools;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterStringOldAndNewAttributeReadTests
    {
        [TestMethod]
        public void ReadingCsv_CanReplaceStringsOnSingleProperty_ValuesConverted()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "SomeText", "OtherText" })
                .Returns(new List<string> { "1", "dog", "hey1" })
                .Returns(new List<string> { "2", "Dog", "hey2" })
                .Returns(new List<string> { "3", "DOG", "hey3" });

            var classUnderTest = new CsvReaderService<CsvConverterStringOldAndNewReadData1>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterStringOldAndNewReadData1 row1 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData1 row2 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData1 row3 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData1 row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual("cat", row1.SomeText);
            Assert.AreEqual("hey1", row1.OtherText, "This text should NOT have been touched");

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Dog", row2.SomeText);
            Assert.AreEqual("hey2", row2.OtherText, "This text should NOT have been touched");

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("DOG", row3.SomeText);
            Assert.AreEqual("hey3", row3.OtherText, "This text should NOT have been touched");

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }


        [TestMethod]
        public void ReadingCsv_ClassLevelPropertiesCanReplaceStringsMoreThanOneProperty_ValuesConverted()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "SomeText", "OtherText" })
                .Returns(new List<string> { "1", "dog", "hey1" })
                .Returns(new List<string> { "2", "Dog", "hey2" })
                .Returns(new List<string> { "3", "Rocks", "hey3" });

            var classUnderTest = new CsvReaderService<CsvConverterStringOldAndNewReadData2>(rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterStringOldAndNewReadData2 row1 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData2 row2 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData2 row3 = classUnderTest.GetRecord();
            CsvConverterStringOldAndNewReadData2 row4 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);
            Assert.AreEqual("cat", row1.SomeText);
            Assert.AreEqual("hey1", row1.OtherText, "This text should NOT have been touched");

            Assert.AreEqual(2, row2.Order);
            Assert.AreEqual("Dog", row2.SomeText);
            Assert.AreEqual("jelly", row2.OtherText);

            Assert.AreEqual(3, row3.Order);
            Assert.AreEqual("Rocks", row3.SomeText);
            Assert.AreEqual("hey3", row3.OtherText, "This text should NOT have been touched");

            Assert.IsNull(row4, "There is no 4th row!");
            rowReaderMock.VerifyAll();
        }

    }

    internal class CsvConverterStringOldAndNewReadData1
    {
        public int Order { get; set; }

        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), OldValue = "dog", NewValue = "cat")]
        public string SomeText { get; set; } = string.Empty;
        public string OtherText { get; set; } = string.Empty;
    }

    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), OldValue = "dog", NewValue = "cat", TargetPropertyType = typeof(string), IsPreConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), OldValue = "hey2", NewValue = "jelly", TargetPropertyType = typeof(string), IsPreConverter = true)]
    internal class CsvConverterStringOldAndNewReadData2
    {
        public int Order { get; set; }
        public string SomeText { get; set; } = string.Empty;
        public string OtherText { get; set; } = string.Empty;
    }
}
