using System;
using CsvConverter.CsvToClass;
using CsvConverter.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Tests
{
    [TestClass]
    public class StringToObjectDefaultConvertersTests
    {
        private DefaultStringToObjectTypeConverterManager _classUnderTest;

        [TestInitialize]
        public void Init()
        {
            _classUnderTest = new DefaultStringToObjectTypeConverterManager();
        }

        #region string
        [TestMethod]
        public void String_CanAssignValues_ValuesAssigned()
        {
            PropertyTester("12345", "12345");
            PropertyTester("Text with spaces", "Text with spaces");
            PropertyTester("", "");
            PropertyTester(string.Empty, String.Empty);
            PropertyTester<string>(null, null);
        }
        #endregion

        #region byte and byte?
        [TestMethod]
        public void Byte_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<byte>(255, "255");
            PropertyTester<byte>(23, "23");
            PropertyTester<byte>(0, "");
            PropertyTester<byte>(0, String.Empty);
            PropertyTester<byte>(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Byte_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<byte>(0, "abc");
            PropertyTester<byte>(0, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableByte_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<byte?>(255, "255");
            PropertyTester<byte?>(34, "34");
            PropertyTester<byte?>(null, "");
            PropertyTester<byte?>(null, String.Empty);
            PropertyTester<byte?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableByte_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<byte?>(null, "abc");
            PropertyTester<byte?>(null, "5488e4$#@#");
        }
        #endregion

        #region int and int?
        [TestMethod]
        public void Integer_CanAssignValues_ValuesAssigned()
        {
            PropertyTester(12345, "12345");
            PropertyTester(12345, "12,345");
            PropertyTester(0, "");
            PropertyTester(0, String.Empty);
            PropertyTester(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Integer_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<int>(0, "abc");
            PropertyTester<int>(0, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableInteger_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<int?>(12345, "12345");
            PropertyTester<int?>(12345, "12,345");
            PropertyTester<int?>(null, "");
            PropertyTester<int?>(null, String.Empty);
            PropertyTester<int?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableInteger_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<int?>(null, "abc");
            PropertyTester<int?>(null, "5488e4$#@#");
        }
        #endregion

        #region double and double?
        [TestMethod]
        public void Double_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<double>(12345.25, "12345.25");
            PropertyTester<double>(12345.25, "12,345.25");
            PropertyTester<double>(0.023, "2.3%");
            PropertyTester<double>(0.0, "");
            PropertyTester<double>(0.0, String.Empty);
            PropertyTester<double>(0.0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Double_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<double>(0, "abc");
            PropertyTester<double>(0, "5488e4$#@#");
        }

        [TestMethod]
        public void Double_CanRoundToGivenPrecision_NumberRoundedProperly()
        {
            _classUnderTest.UpdateDoubleSettings(new DecimalPlacesSettings(2));
            PropertyTester<double>(48.58, "48.5750001");
            PropertyTester<double>(48.57, "48.5749999");
        }

        [TestMethod]
        public void NullableDouble_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<double?>(12345.254, "12345.254");
            PropertyTester<double?>(12345.25, "12,345.25");
            PropertyTester<double?>(0.023, "2.3%");
            PropertyTester<double?>(null, "");
            PropertyTester<double?>(null, String.Empty);
            PropertyTester<double?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableDouble_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<int?>(null, "abc");
            PropertyTester<int?>(null, "5488e4$#@#");
        }
        #endregion

        #region decimal and decimal?
        [TestMethod]
        public void Decimal_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<decimal>(12345.25m, "12345.25");
            PropertyTester<decimal>(12345.25m, "12,345.25");
            PropertyTester<decimal>(0.023m, "2.3%");
            PropertyTester<decimal>(0.0m, "");
            PropertyTester<decimal>(0.0m, String.Empty);
            PropertyTester<decimal>(0.0m, null);
        }

        [TestMethod]
        public void Decimal_CanRoundToGivenPrecision_NumberRoundedProperly()
        {
            _classUnderTest.UpdateDecimalSettings(new DecimalPlacesSettings(2));            
            PropertyTester<decimal>(58.58m, "58.5750001");
            PropertyTester<decimal>(58.57m, "58.5749999");

            _classUnderTest.UpdateDecimalSettings(new DecimalPlacesSettings(1));
            PropertyTester<decimal>(58.6m, "58.5749999");

            _classUnderTest.UpdateDecimalSettings(new DecimalPlacesSettings(0));
            PropertyTester<decimal>(59.0m, "58.5749999");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Decimal_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester(0m, "abc");
            PropertyTester(0m, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableDecimal_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<decimal?>(12345.254m, "12345.254");
            PropertyTester<decimal?>(12345.25m, "12,345.25");
            PropertyTester<decimal?>(0.023m, "2.3%");
            PropertyTester<decimal?>(null, "");
            PropertyTester<decimal?>(null, String.Empty);
            PropertyTester<decimal?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableDecimal_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<int?>(null, "abc");
            PropertyTester<int?>(null, "5488e4$#@#");
        }
        #endregion

        #region bool and bool?
        [TestMethod]
        public void Bool_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<bool>(true, "TRUE");
            PropertyTester<bool>(true, "True");
            PropertyTester<bool>(true, "true");
            PropertyTester<bool>(true, "t");
            PropertyTester<bool>(true, "y");
            PropertyTester<bool>(true, "1");

            PropertyTester<bool>(false, "FALSE");
            PropertyTester<bool>(false, "False");
            PropertyTester<bool>(false, "false");
            PropertyTester<bool>(false, "f");
            PropertyTester<bool>(false, "n");
            PropertyTester<bool>(false, "0");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Bool_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester(false, "abc");
            PropertyTester(false, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableBool_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<bool?>(true, "TRUE");
            PropertyTester<bool?>(true, "True");
            PropertyTester<bool?>(true, "true");
            PropertyTester<bool?>(true, "t");
            PropertyTester<bool?>(true, "y");
            PropertyTester<bool?>(true, "1");

            PropertyTester<bool?>(false, "FALSE");
            PropertyTester<bool?>(false, "False");
            PropertyTester<bool?>(false, "false");
            PropertyTester<bool?>(false, "f");
            PropertyTester<bool?>(false, "n");
            PropertyTester<bool?>(false, "0");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableBool_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<bool?>(false, "abc");
            PropertyTester<bool?>(false, "5488e4$#@#");
        }
        #endregion

        #region DateTime and DateTime?
        [TestMethod]
        public void DateTime_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<DateTime>(new DateTime(2014, 9, 1), "9/1/2014");
            PropertyTester<DateTime>(new DateTime(2014, 11, 1), "11/1/2014");
            PropertyTester<DateTime>(new DateTime(2016, 11, 1), "11/1/2016");
            PropertyTester<DateTime>(new DateTime(2017, 6, 14), "6/14/2017");
            PropertyTester<DateTime>(new DateTime(2017, 6, 15), "6/15/2017");
            PropertyTester<DateTime>(new DateTime(2014, 9, 1), "41883");
            PropertyTester<DateTime>(new DateTime(2014, 11, 1), "41944");
            PropertyTester<DateTime>(new DateTime(2016, 11, 1), "42675");
            PropertyTester<DateTime>(new DateTime(2017, 6, 14), "42900");
            PropertyTester<DateTime>(new DateTime(2017, 6, 15), "42901");
            PropertyTester<DateTime>(DateTime.MinValue, "");
            PropertyTester<DateTime>(DateTime.MinValue, String.Empty);
            PropertyTester<DateTime>(DateTime.MinValue, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DateTime_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<DateTime>(DateTime.MinValue, "abc");
            PropertyTester<DateTime>(DateTime.MinValue, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableDateTime_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<DateTime?>(new DateTime(2014, 9, 1), "41883");
            PropertyTester<DateTime?>(new DateTime(2014, 11, 1), "41944");
            PropertyTester<DateTime?>(new DateTime(2016, 11, 1), "42675");
            PropertyTester<DateTime?>(new DateTime(2017, 6, 14), "42900");
            PropertyTester<DateTime?>(new DateTime(2017, 6, 15), "42901");
            PropertyTester<DateTime?>(null, "");
            PropertyTester<DateTime?>(null, String.Empty);
            PropertyTester<DateTime?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableDateTime_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<int?>(null, "abc");
            PropertyTester<int?>(null, "5488e4$#@#");
        }
        #endregion

        #region long and long?
        [TestMethod]
        public void Long_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<long>(12345, "12345");
            PropertyTester<long>(12345, "12,345");
            PropertyTester<long>(0, "");
            PropertyTester<long>(0, String.Empty);
            PropertyTester<long>(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Long_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<long>(0, "abc");
            PropertyTester<long>(0, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableLong_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<long?>(12345, "12345");
            PropertyTester<long?>(12345, "12,345");
            PropertyTester<long?>(null, "");
            PropertyTester<long?>(null, String.Empty);
            PropertyTester<long?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableLong_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<long?>(null, "abc");
            PropertyTester<long?>(null, "5488e4$#@#");
        }
        #endregion

        #region short and short?
        [TestMethod]
        public void Short_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<short>(12345, "12345");
            PropertyTester<short>(12345, "12,345");
            PropertyTester<short>(0, "");
            PropertyTester<short>(0, String.Empty);
            PropertyTester<short>(0, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Short_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<short>(0, "abc");
            PropertyTester<short>(0, "5488e4$#@#");
        }

        [TestMethod]
        public void NullableShort_CanAssignValues_ValuesAssigned()
        {
            PropertyTester<short?>(12345, "12345");
            PropertyTester<short?>(12345, "12,345");
            PropertyTester<short?>(null, "");
            PropertyTester<short?>(null, String.Empty);
            PropertyTester<short?>(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullableShort_CanHandleNonIntegerStrings_ThrowsException()
        {
            PropertyTester<short?>(null, "abc");
            PropertyTester<short?>(null, "5488e4$#@#");
        }
        #endregion
        
        private void PropertyTester<T>(T expectedValue, string stringInputValue)
        {
            // Act
            object actualValue = _classUnderTest.Convert(typeof(T), stringInputValue, "ColumnName", 0, 1);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
