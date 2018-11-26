using CsvConverter.Reflection;
using System;
using System.Globalization;

namespace CsvConverter
{
    /// <summary>A converter designed to convert DateTime properties to string values.</summary>
    public class CsvConverterDefaultDateTime : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(DateTime) || propertyType == typeof(DateTime?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(DateTime) || propertyType == typeof(DateTime?);
        }

        /// <summary>The format to use with the DateTime ParseExact method.</summary>
        public string DateFormat { get; set; }

        /// <summary>The Date Format Provider used with the DateTie ParseExcat method</summary>
        public IFormatProvider DateFormatProvider { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>The style to use with the DateTime ParseExact method.</summary>
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;

        /// <summary>Converts a DateTime to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            DateTime data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (DateTime?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (DateTime)value;
            }

            return string.IsNullOrWhiteSpace(DateFormat) ? data.ToString() : data.ToString(DateFormat);
        }

        /// <summary>Converts a string to a DateTime</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return DateTime.MinValue;
            }

            if (string.IsNullOrWhiteSpace(DateFormat) == false)
            {
                if (DateTime.TryParseExact(value, DateFormat, DateFormatProvider, DateStyle, out DateTime exactSomeDate))
                {
                    return exactSomeDate;
                }

                ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultDateTime),
                    inputType, value, columnName, columnIndex, rowNumber,
                    $"Since DateFormat ({DateFormat}) was specified, dates must match the format exactly and this field did NOT match!");
            }
            else
            {
                if (DateTime.TryParse(value, out DateTime someDate))
                {
                    return someDate;
                }
                else if (double.TryParse(value, out var someDouble))
                {
                    try
                    {
                        return DateTime.FromOADate(someDouble);
                    }
                    catch
                    {
                        // Throw exception below instead!
                    }
                }
            }

            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultDateTime),
                inputType, value, columnName, columnIndex, rowNumber);
            return null;
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);
            if (attribute != null)
            {
                if (attribute is CsvConverterDateTimeAttribute settings)
                {
                    DateFormat = settings.StringFormat;
                    DateStyle = settings.DateStyle;
                }
            }
        }
    }
}
