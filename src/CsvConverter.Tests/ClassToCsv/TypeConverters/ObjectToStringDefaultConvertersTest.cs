using System;
using CsvConverter.ClassToCsv;
using CsvConverter.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests.Shared
{
    [TestClass]
    public class ObjectToStringDefaultConvertersTest
    {
        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("Data", "Data")]
        public void CanConvertString(string inputData, string expectedData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeStringProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeStringProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeStringProperty, string.Empty);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #region DateTime and DateTime?
        [DataTestMethod]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", "")]
        [DataRow("5/27/2017", "2017-05-27T00:00:00", "s")]
        [DataRow("1/1/2017", "January 01", "m")]
        [DataRow("1/1/2017", "January, 2017", "y")]
        public void CanConvertDateTime(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeDateTimeProperty = DateTime.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeDateTimeProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDateTimeProperty, formatData);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", null)]
        [DataRow("1/1/2017", "1/1/2017 12:00:00 AM", "")]
        [DataRow("5/27/2017", "2017-05-27T00:00:00", "s")]
        [DataRow("1/1/2017", "January 01", "m")]
        [DataRow("1/1/2017", "January, 2017", "y")]
        public void CanConvertNullableDateTime(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableDateTimeProperty = string.IsNullOrWhiteSpace(inputData) ? (DateTime?)null : DateTime.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableDateTimeProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableDateTimeProperty, formatData);

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
        [DataRow("20", "2,000 %", "P0")]
        public void CanConvertByte(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeByteProperty = byte.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeByteProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeByteProperty, formatData);

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
        [DataRow("20", "2,000 %", "P0")]
        public void CanConvertNullableByte(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableByteProperty = string.IsNullOrWhiteSpace(inputData) ? null : (byte?)byte.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableByteProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableByteProperty, formatData);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region Boolean and Boolean?
        [DataTestMethod]
        [DataRow(true, "1", ConverterSettingForBooleansEnum.UseOneAndZero)]
        [DataRow(false, "0", ConverterSettingForBooleansEnum.UseOneAndZero)]
        [DataRow(true, "T", ConverterSettingForBooleansEnum.UseTandF)]
        [DataRow(false, "F", ConverterSettingForBooleansEnum.UseTandF)]
        [DataRow(true, "True", ConverterSettingForBooleansEnum.UseTrueAndFalse)]
        [DataRow(false, "False", ConverterSettingForBooleansEnum.UseTrueAndFalse)]
        [DataRow(true, "Y", ConverterSettingForBooleansEnum.UseYandN)]
        [DataRow(false, "N", ConverterSettingForBooleansEnum.UseYandN)]
        [DataRow(true, "Yes", ConverterSettingForBooleansEnum.UseYesAndNo)]
        [DataRow(false, "No", ConverterSettingForBooleansEnum.UseYesAndNo)]
        public void CanConvertBoolean(bool inputData, string expectedData, ConverterSettingForBooleansEnum settingForBooleans)
        {
            // Arrange
            var data = new TypeToStringConverterTester();

            data.SomeBoolProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeBoolProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();
            classUnderTest.BooleanSetting = settingForBooleans;

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeBoolProperty, null);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [DataTestMethod]
        [DataRow(null, null, ConverterSettingForBooleansEnum.UseOneAndZero)]
        [DataRow("true", "1", ConverterSettingForBooleansEnum.UseOneAndZero)]
        [DataRow("false", "0", ConverterSettingForBooleansEnum.UseOneAndZero)]
        [DataRow("true", "T", ConverterSettingForBooleansEnum.UseTandF)]
        [DataRow("false", "F", ConverterSettingForBooleansEnum.UseTandF)]
        [DataRow("true", "True", ConverterSettingForBooleansEnum.UseTrueAndFalse)]
        [DataRow("false", "False", ConverterSettingForBooleansEnum.UseTrueAndFalse)]
        [DataRow("true", "Y", ConverterSettingForBooleansEnum.UseYandN)]
        [DataRow("false", "N", ConverterSettingForBooleansEnum.UseYandN)]
        [DataRow("true", "Yes", ConverterSettingForBooleansEnum.UseYesAndNo)]
        [DataRow("false", "No", ConverterSettingForBooleansEnum.UseYesAndNo)]
        public void CanConvertNullableBoolean(string inputData, string expectedData, ConverterSettingForBooleansEnum settingForBooleans)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableBoolProperty = string.IsNullOrWhiteSpace(inputData) ? null : (bool?)bool.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableBoolProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();
            classUnderTest.BooleanSetting = settingForBooleans;

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableBoolProperty, null);

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
        [DataRow("20", "2,000 %", "P0")]
        public void CanConvertShort(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeShortProperty = short.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeShortProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeShortProperty, formatData);

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
        [DataRow("20", "2,000 %", "P0")]
        public void CanConvertNullableShort(string inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableShortProperty = string.IsNullOrWhiteSpace(inputData) ? null : (short?)short.Parse(inputData);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableShortProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableShortProperty, formatData);

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
        [DataRow(20, "2,000 %", "P0")]
        public void CanConvertInt(int inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeIntProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeIntProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeIntProperty, formatData);

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
        [DataRow(20, "2,000 %", "P0")]
        public void CanConvertNullableInt(int? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableIntProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableIntProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableIntProperty, formatData);

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
        [DataRow(20L, "2,000 %", "P0")]
        public void CanConvertLong(long inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeLongProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeLongProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeLongProperty, formatData);

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
        [DataRow(20L, "2,000 %", "P0")]
        public void CanConvertNullableLong(long? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableLongProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableLongProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableLongProperty, formatData);

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
        [DataRow(".20", "20 %", "P0")]
        public void CanConvertDecimal(string inputString, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeDecimalProperty = decimal.Parse(inputString);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeDecimalProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDecimalProperty, formatData);

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
        [DataRow(".20", "20 %", "P0")]
        public void CanConvertNullabeDecimal(string inputString, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullabeDecimalProperty = string.IsNullOrWhiteSpace(inputString) ? null : (decimal?)decimal.Parse(inputString);

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullabeDecimalProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullabeDecimalProperty, formatData);

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
        [DataRow(.20, "20 %", "P0")]
        public void CanConvertDouble(double inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeDoubleProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeDoubleProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeDoubleProperty, formatData);

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
        [DataRow(.20, "20 %", "P0")]
        public void CanConvertNullableDouble(double? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableDoubleProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableDoubleProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableDoubleProperty, formatData);

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
        [DataRow(.20f, "20 %", "P0")]
        public void CanConvertFloat(float inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeFloatProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeFloatProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeFloatProperty, formatData);

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
        [DataRow(.20f, "20 %", "P0")]
        public void CanConvertNullableFloat(float? inputData, string expectedData, string formatData)
        {
            // Arrange
            var data = new TypeToStringConverterTester();
            data.SomeNullableFloatProperty = inputData;

            var propInfo = ReflectionHelper.FindPropertyInfoByName<TypeToStringConverterTester>(nameof(data.SomeNullableFloatProperty));
            var classUnderTest = new ObjectToStringDefaultConverters();

            // Act
            string actualData = classUnderTest.Convert(propInfo.PropertyType, data.SomeNullableFloatProperty, formatData);

            // Assert
            Assert.AreEqual(expectedData, actualData);
        } 
        #endregion
    }

    internal class TypeToStringConverterTester
    {
        public String SomeStringProperty { get; set; }
        public DateTime SomeDateTimeProperty { get; set; }
        public DateTime? SomeNullableDateTimeProperty { get; set; }

        public bool SomeBoolProperty { get; set; }
        public bool? SomeNullableBoolProperty { get; set; }

        public byte SomeByteProperty { get; set; }
        public byte? SomeNullableByteProperty { get; set; }

        public int SomeIntProperty { get; set; }
        public int? SomeNullableIntProperty { get; set; }

        public long SomeLongProperty { get; set; }
        public long? SomeNullableLongProperty { get; set; }

        public short SomeShortProperty { get; set; }
        public short? SomeNullableShortProperty { get; set; }

        public decimal SomeDecimalProperty { get; set; }
        public decimal? SomeNullabeDecimalProperty { get; set; }

        public double SomeDoubleProperty { get; set; }
        public double? SomeNullableDoubleProperty { get; set; }

        public float SomeFloatProperty { get; set; }
        public float? SomeNullableFloatProperty { get; set; }

    }
}
