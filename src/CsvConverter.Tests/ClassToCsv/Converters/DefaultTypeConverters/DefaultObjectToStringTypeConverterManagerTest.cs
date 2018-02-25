using System;
using CsvConverter.ClassToCsv;
using CsvConverter.Shared;
using CsvConverter.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Shared
{
    [TestClass]
    public class DefaultObjectToStringTypeConverterManagerTest
    {
        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("Data", "Data")]
        public void Convert_CanConvertString_StringsConvertered(string inputData, string expectedData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeStringProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeStringProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeStringProperty, string.Empty, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #region DateTime and DateTime?
        [DataTestMethod]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", "")]
        [DataRow("5/27/2017", "2017-05-27T00:00:00", "s")]
        [DataRow("1/12/2017", "January 12", "m")]
        // [DataRow("1/1/2017", "January, 2017", "y")] // Differs between windows 10 (January 2017) and Windows 7 (January, 2017)
        public void Convert_CanConvertDateTime_DatesConverted(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeDateTimeProperty = DateTime.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeDateTimeProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDateTimeProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", "")]
        [DataRow("5/27/2017", "2017-05-27T00:00:00", "s")]
        [DataRow("1/12/2017", "January 12", "m")]
        // [DataRow("1/1/2017", "January, 2017", "y")]  // Differs between windows 10 (January 2017) and Windows 7 (January, 2017)
        public void Convert_CanConvertNullableDateTime_DatesConverted(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableDateTimeProperty = string.IsNullOrWhiteSpace(inputData) ? (DateTime?)null : DateTime.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableDateTimeProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableDateTimeProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
        #endregion

        #region Byte and Byte?
        [DataTestMethod]
        [DataRow("1", "1", null)]
        [DataRow("23", "23", null)]
        [DataRow("255", "255", null)]
        [DataRow("255", "255", "")]
        [DataRow("255", "255", "N0")]
        [DataRow("255", "255.0", "N1")]
        [DataRow("255", "255.00", "N2")]
        // [DataRow("20", "2,000 %", "P0")] // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertByte_BytesConverted(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeByteProperty = byte.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeByteProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeByteProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1", "1", null)]
        [DataRow("23", "23", null)]
        [DataRow("255", "255", null)]
        [DataRow("255", "255", "")]
        [DataRow("255", "255", "N0")]
        [DataRow("255", "255.0", "N1")]
        [DataRow("255", "255.00", "N2")]
       // [DataRow("20", "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertNullableByte_BytesConverted(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableByteProperty = string.IsNullOrWhiteSpace(inputData) ? null : (byte?)byte.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableByteProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableByteProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
        #endregion

        #region Boolean and Boolean?
        [DataTestMethod]
        [DataRow(true, "1", BooleanOutputFormatEnum.UseOneAndZero)]
        [DataRow(false, "0", BooleanOutputFormatEnum.UseOneAndZero)]
        [DataRow(true, "T", BooleanOutputFormatEnum.UseTandF)]
        [DataRow(false, "F", BooleanOutputFormatEnum.UseTandF)]
        [DataRow(true, "True", BooleanOutputFormatEnum.UseTrueAndFalse)]
        [DataRow(false, "False", BooleanOutputFormatEnum.UseTrueAndFalse)]
        [DataRow(true, "Y", BooleanOutputFormatEnum.UseYandN)]
        [DataRow(false, "N", BooleanOutputFormatEnum.UseYandN)]
        [DataRow(true, "Yes", BooleanOutputFormatEnum.UseYesAndNo)]
        [DataRow(false, "No", BooleanOutputFormatEnum.UseYesAndNo)]
        public void Convert_CanConvertBoolean_BoolConverted(bool inputData, string expectedData, BooleanOutputFormatEnum settingForBooleans)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();

            data.SomeBoolProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeBoolProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();
            classUnderTest.UpdateBooleanSettings(new BooleanConverterSettings() { OutputFormat = settingForBooleans });

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeBoolProperty, null, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, BooleanOutputFormatEnum.UseOneAndZero)]
        [DataRow("true", "1", BooleanOutputFormatEnum.UseOneAndZero)]
        [DataRow("false", "0", BooleanOutputFormatEnum.UseOneAndZero)]
        [DataRow("true", "T", BooleanOutputFormatEnum.UseTandF)]
        [DataRow("false", "F", BooleanOutputFormatEnum.UseTandF)]
        [DataRow("true", "True", BooleanOutputFormatEnum.UseTrueAndFalse)]
        [DataRow("false", "False", BooleanOutputFormatEnum.UseTrueAndFalse)]
        [DataRow("true", "Y", BooleanOutputFormatEnum.UseYandN)]
        [DataRow("false", "N", BooleanOutputFormatEnum.UseYandN)]
        [DataRow("true", "Yes", BooleanOutputFormatEnum.UseYesAndNo)]
        [DataRow("false", "No", BooleanOutputFormatEnum.UseYesAndNo)]
        public void Convert_CanConvertNullableBoolean_BoolConverted(string inputData, string expectedData, BooleanOutputFormatEnum settingForBooleans)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableBoolProperty = string.IsNullOrWhiteSpace(inputData) ? null : (bool?)bool.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableBoolProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();
            classUnderTest.UpdateBooleanSettings(new BooleanConverterSettings() { OutputFormat = settingForBooleans });

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableBoolProperty, null, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
        #endregion

        #region Short and Short?
        [DataTestMethod]
        [DataRow("1", "1", null)]
        [DataRow("23", "23", null)]
        [DataRow("2000", "2000", null)]
        [DataRow("2000", "2,000", "N0")]
        [DataRow("2000", "2,000.0", "N1")]
        [DataRow("2000", "2,000.00", "N2")]
        //  [DataRow("20", "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertShort_ShortConverted(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeShortProperty = short.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeShortProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeShortProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1", "1", null)]
        [DataRow("23", "23", null)]
        [DataRow("2000", "2000", null)]
        [DataRow("2000", "2,000", "N0")]
        [DataRow("2000", "2,000.0", "N1")]
        [DataRow("2000", "2,000.00", "N2")]
        //   [DataRow("20", "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertNullableShort_ShortConverted(string inputData, string expectedData, string formatData)
        {

            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableShortProperty = string.IsNullOrWhiteSpace(inputData) ? null : (short?)short.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableShortProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableShortProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region Int and Int?
        [DataTestMethod]
        [DataRow(1, "1", null)]
        [DataRow(23, "23", null)]
        [DataRow(2000, "2000", null)]
        [DataRow(2000, "2,000", "N0")]
        [DataRow(2000, "2,000.0", "N1")]
        [DataRow(2000, "2,000.00", "N2")]
        //   [DataRow(20, "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertInt_IntConverted(int inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeIntProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeIntProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeIntProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1, "1", null)]
        [DataRow(23, "23", null)]
        [DataRow(2000, "2000", null)]
        [DataRow(2000, "2,000", "N0")]
        [DataRow(2000, "2,000.0", "N1")]
        [DataRow(2000, "2,000.00", "N2")]
        //  [DataRow(20, "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertNullableInt_IntConverted(int? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableIntProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableIntProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableIntProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region long and long?
        [DataTestMethod]
        [DataRow(1L, "1", null)]
        [DataRow(23L, "23", null)]
        [DataRow(2000L, "2000", null)]
        [DataRow(2000L, "2,000", "N0")]
        [DataRow(2000L, "2,000.0", "N1")]
        [DataRow(2000L, "2,000.00", "N2")]
        //  [DataRow(20L, "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertLong_LongConverted(long inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeLongProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeLongProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeLongProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1L, "1", null)]
        [DataRow(23L, "23", null)]
        [DataRow(2000L, "2000", null)]
        [DataRow(2000L, "2,000", "N0")]
        [DataRow(2000L, "2,000.0", "N1")]
        [DataRow(2000L, "2,000.00", "N2")]
        //  [DataRow(20L, "2,000 %", "P0")]  // Differs between windows 10 (2,000%) & windows 7 (2,000 %)
        public void Convert_CanConvertNullableLong_LongConverted(long? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableLongProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableLongProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableLongProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region decimal and decimal?
        [DataTestMethod]
        [DataRow("1.0", "1.0", null)]
        [DataRow("23.0", "23.0", null)]
        [DataRow("2000.0", "2000.0", null)]
        [DataRow("2000.0", "2,000.00", "N")]
        [DataRow("2000.0", "2,000", "N0")]
        [DataRow("2000.0", "2,000.0", "N1")]
        [DataRow("2000.0", "2,000.00", "N2")]
        //  [DataRow(".20", "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertDecimal_DecimalConverted(string inputString, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeDecimalProperty = decimal.Parse(inputString);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeDecimalProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDecimalProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1.0", "1.0", null)]
        [DataRow("23.0", "23.0", null)]
        [DataRow("2000.0", "2000.0", null)]
        [DataRow("2000.0", "2,000.00", "N")]
        [DataRow("2000.0", "2,000", "N0")]
        [DataRow("2000.0", "2,000.0", "N1")]
        [DataRow("2000.0", "2,000.00", "N2")]
     //   [DataRow(".20", "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertNullabeDecimal_DecimalConverted(string inputString, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullabeDecimalProperty = string.IsNullOrWhiteSpace(inputString) ? null : (decimal?)decimal.Parse(inputString);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullabeDecimalProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullabeDecimalProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }
        #endregion

        #region double and double?

        [DataTestMethod]
        [DataRow(1.0, "1", null)]
        [DataRow(23.0, "23", null)]
        [DataRow(2000.0, "2000", null)]
        [DataRow(2000.0, "2,000.00", "N")]
        [DataRow(2000.0, "2,000", "N0")]
        [DataRow(2000.0, "2,000.0", "N1")]
        [DataRow(2000.0, "2,000.00", "N2")]
        //  [DataRow(.20, "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertDouble_DoubleConverted(double inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeDoubleProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeDoubleProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDoubleProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1.0, "1", null)]
        [DataRow(23.0, "23", null)]
        [DataRow(2000.0, "2000", null)]
        [DataRow(2000.0, "2,000.00", "N")]
        [DataRow(2000.0, "2,000", "N0")]
        [DataRow(2000.0, "2,000.0", "N1")]
        [DataRow(2000.0, "2,000.00", "N2")]
        //   [DataRow(.20, "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertNullableDouble_DoubleConverted(double? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableDoubleProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableDoubleProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableDoubleProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region float and float?
        [DataTestMethod]
        [DataRow(1.0f, "1", null)]
        [DataRow(23.0f, "23", null)]
        [DataRow(2000.0f, "2000", null)]
        [DataRow(2000.0f, "2,000.00", "N")]
        [DataRow(2000.0f, "2,000", "N0")]
        [DataRow(2000.0f, "2,000.0", "N1")]
        [DataRow(2000.0f, "2,000.00", "N2")]
        //  [DataRow(.20f, "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertFloat_FloatConverted(float inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeFloatProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeFloatProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeFloatProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(1.0f, "1", null)]
        [DataRow(23.0f, "23", null)]
        [DataRow(2000.0f, "2000", null)]
        [DataRow(2000.0f, "2,000.00", "N")]
        [DataRow(2000.0f, "2,000", "N0")]
        [DataRow(2000.0f, "2,000.0", "N1")]
        [DataRow(2000.0f, "2,000.00", "N2")]
        //    [DataRow(.20f, "20 %", "P0")]  // Differs between windows 10 (20%) & windows 7 (20 %)
        public void Convert_CanConvertNullableFloat_FloatConverted(float? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new DefaultTypeConverterTestData();
            data.SomeNullableFloatProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<DefaultTypeConverterTestData>(nameof(data.SomeNullableFloatProperty));
            var classUnderTest = new DefaultObjectToStringTypeConverterManager();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableFloatProperty, formatData, "Column1", 1, 1);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        } 
        #endregion
    }
}
