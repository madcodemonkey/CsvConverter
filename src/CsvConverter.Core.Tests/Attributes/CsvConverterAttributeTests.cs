using System;
using System.Collections.Generic;
using System.Text;
using CsvConverter.RowTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CsvConverter.Core.Tests.Attributes
{
    [TestClass]
    public class CsvConverterAttributeTests
    {
        [TestMethod]
        public void YouCanSpecifyBothAnIgnoreReadAndWriteOnAnObjectAndNotGetAnException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Order", "Tall" })
                .Returns(new List<string> { "1", "true", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData1>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterAttributeData1 row1 = classUnderTest.GetRecord();
            CsvConverterAttributeData1 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void IfTheUserDoesNotSpecifyAConverterOneIsCreatedAndUsed()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Number", "Tall" })
                .Returns(new List<string> { "1", "true", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData3>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterAttributeData3 row1 = classUnderTest.GetRecord();
            CsvConverterAttributeData3 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual(1, row1.Order);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        public void UserCanHaveOneConveterForReadingAndAnotherForWriting()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FirstName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData4>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            CsvConverterAttributeData4 row1 = classUnderTest.GetRecord();
            CsvConverterAttributeData4 row2 = classUnderTest.GetRecord();

            // Assert
            Assert.AreEqual("John", row1.FirstName);
            Assert.AreEqual(" Doe ", row1.LastName);

            Assert.IsNull(row2, "There is no 2nd row!");
            rowReaderMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void IfColumnIndexIsSpecifiedAtTheClassLevel_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Name" })
                .Returns(new List<string> { "John" });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData7>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();
            
            // Assert
            Assert.Fail("An exception should have been genearted for using ColumnIndex at the class level");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void IfAltColumnNamesIsSpecifiedAtTheClassLevel_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Name" })
                .Returns(new List<string> { "John" });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData7>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("An exception should have been genearted for using ColumnIndex at the class level");
        }


        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void IfColumnNameIsSpecifiedAtTheClassLevel_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "Name" })
                .Returns(new List<string> { "John" });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData6>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("An exception should have been genearted for using ColumnIndex at the class level");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void IfTheUserSpecifiesTheColumnNameOnMoreThanOneAttribute_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData8>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the column name twice");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingColumnNameOnAPreConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData9>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the ColumnName on a pre-converter");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingColumnNameOnAPostConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData10>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the ColumnName on a post-converter");
        }


        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingColumnIndexOnAPreConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData11>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the ColumnIndex on a pre-converter");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingColumnIndexOnAPostConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData12>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the ColumnIndex on a post-converter");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingAltColumnNamesOnAPreConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData13>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the AltColumnNames on a pre-converter");
        }

        [TestMethod]
        [ExpectedException(typeof(CsvConverterAttributeException))]
        public void SpecifyingAltColumnNamesOnAPostConverter_YouGetCsvConverterAttributeException()
        {
            // Arrange
            var rowReaderMock = new Mock<IRowReader>();
            rowReaderMock.SetupSequence(m => m.CanRead()).Returns(true).Returns(true).Returns(false);
            rowReaderMock.Setup(m => m.IsRowBlank).Returns(false);
            rowReaderMock.SetupSequence(m => m.ReadRow())
                .Returns(new List<string> { "FName", "LastName" })
                .Returns(new List<string> { " John ", " Doe ", });

            var classUnderTest = new CsvReaderService<CsvConverterAttributeData14>(
                rowReaderMock.Object);
            classUnderTest.Configuration.HasHeaderRow = true;

            // Act
            var row1 = classUnderTest.GetRecord();

            // Assert
            Assert.Fail("Should have got an error for specifing the AltColumnNames on a post-converter");
        }

    }

    public class CsvConverterAttributeData2
    {
        public int Age { get; set; }
    }

    public class CsvConverterAttributeData1
    {
        public int Order { get; set; }
        public bool Tall { get; set; }

        [CsvConverter(IgnoreWhenReading = true, IgnoreWhenWriting = true)]
        public CsvConverterAttributeData2 Other1 { get; set; }

        // You don't need the attribute!
        public CsvConverterAttributeData2 Other2 { get; set; }
    }

    public class CsvConverterAttributeData3
    {
        [CsvConverter(ColumnName = "Number")]
        public int Order { get; set; }
        public bool Tall { get; set; }
    }

    public class CsvConverterAttributeData4
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), IgnoreWhenWriting = true)]
        [CsvConverter(IgnoreWhenReading = true)] // default converter
        public string FirstName { get; set; }


        [CsvConverter(IgnoreWhenWriting = true)] // default converter
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), IgnoreWhenReading = true)]
        public string LastName { get; set; }
    }

    [CsvConverter(TargetPropertyType = typeof(string), ColumnIndex = 1)]
    public class CsvConverterAttributeData5
    {
        public string Name { get; set; }
    }

    [CsvConverter(TargetPropertyType = typeof(string), ColumnName = "Stuff2")]
    public class CsvConverterAttributeData6
    {
        public string Stuff { get; set; }
    }

    [CsvConverter(TargetPropertyType = typeof(string), AltColumnNames = "Stuff2")]
    public class CsvConverterAttributeData7
    {
        public string Stuff { get; set; }
    }

    public class CsvConverterAttributeData8
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), ColumnName = "FName", IgnoreWhenWriting = true)]
        [CsvConverter(ColumnName = "FName", IgnoreWhenReading = true)] 
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CsvConverterAttributeData9
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), ColumnName = "FName", IsPreConverter = true)]
         public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CsvConverterAttributeData10
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), ColumnName = "FName", IsPostConverter = true)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CsvConverterAttributeData11
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), ColumnIndex = 1, IsPreConverter = true)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CsvConverterAttributeData12
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), ColumnIndex = 1, IsPostConverter = true)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }


    public class CsvConverterAttributeData13
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), AltColumnNames = "FName", IsPreConverter = true)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class CsvConverterAttributeData14
    {
        [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), AltColumnNames = "FName", IsPostConverter = true)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

}
