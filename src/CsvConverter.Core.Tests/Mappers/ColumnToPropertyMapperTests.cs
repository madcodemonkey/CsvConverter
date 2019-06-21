using System.Collections.Generic;
using System.Linq;
using CsvConverter.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.Tests.HeaderTests
{
    [TestClass]
    public class ColumnToPropertyMapperTests
    {
        [TestMethod]
        public void IgnoreWhenReading_WhenSpecifiedByItself_InOneAttribute_OneMapIsCreatedAndIgnoreWhenReadingIsTrue()
        {
            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            var listOfHeaders = new List<string>() { "Data1", "Data2", "Order" };

            // Act
            List<ColumnToPropertyMap> maps =  classUnderTest.CreateReadMap(listOfHeaders);

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "Order").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsTrue(map.IgnoreWhenReading);
            Assert.IsNull(map.ReadConverter);
        }

        [TestMethod]
        public void IgnoreWhenReading_WhenSpecifiedWith_IgnoreWhenWriting_InOneAttribute_OneMapIsCreatedAndIgnoreWhenReadingIsTrue()
        {
            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            var listOfHeaders = new List<string>() { "Data1", "Data2", "Length" };

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateReadMap(listOfHeaders);

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "Length").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsTrue(map.IgnoreWhenReading);
            Assert.IsNull(map.ReadConverter);
        }

        [TestMethod]
        public void IgnoreWhenReading_WhenSpecifiedWith_IgnoreWhenWriting_InTwoAttributes_OneMapIsCreatedAndIgnoreWhenReadingIsFalse()
        {
            // Why?
            // You got both a READ and WRITE converter here!  You basically told the mapper that you wanted the 
            // the CsvConverterNumber converter for reading and writing on this property type.  The purpose behind letting a user
            // specify two converter attributes is so that they could be DIFFERENT for reading and writing.  If your intent
            // is to tell the mapper to ignore it use  [CsvConverterNumber(IgnoreWhenReading = true, IgnoreWhenWriting = true)] instead!

            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            var listOfHeaders = new List<string>() { "Data1", "Data2", "PercentageMuscle" };

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateReadMap(listOfHeaders);

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "PercentageMuscle").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsFalse(map.IgnoreWhenReading);
            Assert.IsNotNull(map.ReadConverter);
        }

        // 
        [TestMethod]
        public void IgnoreWhenReading_WhenSpecifiedWithBothAsFalse_OneMapIsCreatedAndIgnoreWhenReadingIsFalse()
        {
            // It's a stupid thing to do, but its legal

            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            var listOfHeaders = new List<string>() { "Data1", "Data2", "HasJumperCables" };

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateReadMap(listOfHeaders);

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "HasJumperCables").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsFalse(map.IgnoreWhenReading);
            Assert.IsNotNull(map.ReadConverter);
        }


        [TestMethod]
        public void IgnoreWhenWriting_WhenSpecifiedByItself_InOneAttribute_NoMapIsCreated()
        {
            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateWriteMap();

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "PercentageBodyFat").FirstOrDefault();
            Assert.IsNull(map);
        }

        [TestMethod]
        public void IgnoreWhenWriting_WhenSpecifiedWith_IgnoreWhenReading_InOneAttribute_NoMapIsCreated()
        {
            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateWriteMap();

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "Length").FirstOrDefault();
            Assert.IsNull(map);
        }

        [TestMethod]
        public void IgnoreWhenWriting_WhenSpecifiedWith_IgnoreWhenWriting_InTwoAttributes_OneMapIsCreatedAndIgnoreWhenWritingIsFalse()
        {
            // Why?
            // You got both a READ and WRITE converter here!  You basically told the mapper that you wanted the 
            // the CsvConverterNumber converter for reading and writing on this property type.  The purpose behind letting a user
            // specify two converter attributes is so that they could be DIFFERENT for reading and writing.  If your intent
            // is to tell the mapper to ignore it use  [CsvConverterNumber(IgnoreWhenReading = true, IgnoreWhenWriting = true)] instead!

            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateWriteMap();

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "PercentageMuscle").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsFalse(map.IgnoreWhenWriting);
            Assert.IsNotNull(map.WriteConverter);
        }

        [TestMethod]
        public void IgnoreWhenWriting_WhenSpecifiedWithBothAsFalse_OneMapIsCreatedAndIgnoreWhenWritingIsFalse()
        {
            // It's a stupid thing to do, but it's legal.  

            // Arrange
            var classUnderTest = new ColumnToPropertyMapper<ColumnToPropertyMapperTestsData1>(
                new CsvConverterConfiguration(), new DefaultTypeConverterFactory(), -1);

            // Act
            List<ColumnToPropertyMap> maps = classUnderTest.CreateWriteMap();

            // Assert
            ColumnToPropertyMap map = maps.Where(w => w.ColumnName == "HasJumperCables").SingleOrDefault();
            Assert.IsNotNull(map);
            Assert.IsFalse(map.IgnoreWhenWriting);
            Assert.IsNotNull(map.WriteConverter);
        }

    }

    internal class ColumnToPropertyMapperTestsData1
    {
        [CsvConverterNumber(NumberOfDecimalPlaces = 4)]
        public double Data1 { get; set; }

        [CsvConverterNumber(IgnoreWhenReading = true)]
        public int Order { get; set; }

        [CsvConverterNumber(IgnoreWhenWriting = true)]
        public double PercentageBodyFat { get; set; }

        [CsvConverterNumber(IgnoreWhenReading = true)]
        [CsvConverterNumber(IgnoreWhenWriting = true)]
        public double PercentageMuscle { get; set; }

        [CsvConverterNumber(IgnoreWhenReading = true, IgnoreWhenWriting = true)]
        public double Length { get; set; }

        [CsvConverterNumber(IgnoreWhenReading = false, IgnoreWhenWriting = false)]
        public bool HasJumperCables { get; set; }


        [CsvConverterNumber(NumberOfDecimalPlaces = 4)]
        public double Data2 { get; set; }
    } 

}
