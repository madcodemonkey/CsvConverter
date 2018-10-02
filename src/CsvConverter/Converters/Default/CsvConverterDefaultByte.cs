using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>A converter designed to convert byte properties to string values.</summary>
    public class CsvConverterDefaultByte : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(byte) || propertyType == typeof(byte?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(byte) || propertyType == typeof(byte?);
        }

        /// <summary>The output format to use when converting a number into a string</summary>
        public string StringFormat { get; set; }


        /// <summary>Converts a byte to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            byte data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (byte?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (byte)value;
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

                return (byte)0;
            }

            if (byte.TryParse(value, out byte number))
            {
                return number;
            }


            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultByte),
                inputType, value, columnName, columnIndex, rowNumber);
            return (byte)0; // never reached
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
