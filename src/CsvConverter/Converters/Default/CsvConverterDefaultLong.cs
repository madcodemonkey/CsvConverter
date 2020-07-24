using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>A converter designed to convert long properties to string values.</summary>
    public class CsvConverterDefaultLong :  CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specified?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(long) || propertyType == typeof(long?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(long) || propertyType == typeof(long?);
        }

        /// <summary>The output format to use when converting a number into a string</summary>
        public string StringFormat { get; set; }


        /// <summary>Converts a long to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            long data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (long?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (long)value;
            }

            return string.IsNullOrWhiteSpace(StringFormat) ? data.ToString() : data.ToString(StringFormat);
        }

        /// <summary>Converts a string to a long</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return (long)0;
            }

            if (long.TryParse(value, out long number))
            {
                return number;
            }
            else if (value.IndexOf(",") > -1)
            {
                // There are commas in the value. Try removing them.
                var noComma = value.Replace(",", "");
                return GetReadData(inputType, noComma, columnName, columnIndex, rowNumber);
            }

            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultLong),
                inputType, value, columnName, columnIndex, rowNumber);
            return (long)0;
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
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
