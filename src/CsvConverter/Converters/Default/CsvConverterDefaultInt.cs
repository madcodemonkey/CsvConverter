using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>A converter for turning strings into integer values.</summary>
    public class CsvConverterDefaultInt : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(int) || propertyType == typeof(int?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(int) || propertyType == typeof(int?);
        }

        /// <summary>The output format to use when converting a number into a string</summary>
        public string StringFormat { get; set; }


        /// <summary>Converts a byte to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            int data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (int?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (int)value;
            }

            return string.IsNullOrWhiteSpace(StringFormat) ? data.ToString() : data.ToString(StringFormat);
        }

        /// <summary>Converts a string to a byte</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return 0;
            }

            if (int.TryParse(value, out int number))
            {
                return number;
            }
            else if (value.IndexOf(",") > -1)
            {
                // There are commas in the value. Try removing them.
                var noComma = value.Replace(",", "");
                return GetReadData(inputType, noComma, columnName, columnIndex, rowNumber);
            }

            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultInt),
                inputType, value, columnName, columnIndex, rowNumber);
            return 0;
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterBaseAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);
            if (attribute != null)
            {
                if (attribute is CsvConverterNumberAttribute settings)
                {
                    StringFormat = settings.StringFormat;
                }
            }
        }
    }

}
