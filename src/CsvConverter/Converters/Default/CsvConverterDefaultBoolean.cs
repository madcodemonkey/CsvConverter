using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>A converter designed to convert boolean properties to string values.</summary>
    public class CsvConverterDefaultBoolean : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(bool) || propertyType == typeof(bool?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(bool) || propertyType == typeof(bool?);
        }

        /// <summary>Indicates the text that represents a true value in the CSV file (it is case insensitive for inputs and written as is for outputs)</summary>
        public string TrueValue { get; set; } = "True";

        /// <summary>Indicates the text that represents a false value in the CSV file (it is case insensitive for inputs and written as is for outputs)</summary>
        public string FalseValue { get; set; } = "False";

        /// <summary>Converts a string to a boolean</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return false;
            }

            if (bool.TryParse(value, out bool booleanValue))
            {
                return booleanValue;
            }

            var lower = value.Trim().ToLower();
            if (lower == TrueValue || lower == "true" || lower == "t" || lower == "y" || lower == "yes" || lower == "1")
            {
                return true;
            }

            if (lower == FalseValue || lower == "false" || lower == "f" || lower == "n" || lower == "no" || lower == "0")
            {
                return false;
            }

            ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultBoolean),
                inputType, value, columnName, columnIndex, rowNumber);

            return false; // Never reached!
        }

        /// <summary>Converts a boolean to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            bool data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (bool?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (bool)value;
            }

            return data ? TrueValue : FalseValue;
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterBaseAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);
            if (attribute != null)
            {
                var settings = attribute as CsvConverterBooleanAttribute;
                if (settings != null)
                {
                    TrueValue = settings.TrueValue;
                    FalseValue = settings.FalseValue;
                }
            }
        }



    }
}
