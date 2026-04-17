using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>A converter for guids.</summary>
    public class CsvConverterDefaultGuid : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specified?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(Guid) || propertyType == typeof(Guid?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(Guid) || propertyType == typeof(Guid?);
        }

        /// <summary>Returns value</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            return value?.ToString();
        }

        /// <summary>Returns value</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return Guid.Empty;
            }

            if (Guid.TryParse(value, out Guid guid))
                return guid;

            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultGuid),
                inputType, value, columnName, columnIndex, rowNumber);

            return null; // Not called, but removes compiler warning
        }
    }
}
