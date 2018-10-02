using CsvConverter.Reflection;
using System;
using System.Globalization;

namespace CsvConverter
{
    /// <summary>A converter designed to convert double properties to string values.</summary>
    public class CsvConverterDefaultDouble : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(double) || propertyType == typeof(double?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(double) || propertyType == typeof(double?);
        }

        /// <summary>Mode is used when AllowRounding is true.  It is used with the Math.Round function.</summary>
        public MidpointRounding Mode { get; set; } = MidpointRounding.AwayFromZero;

        /// <summary>Default is TRUE and if true, Mode is used with Math.Round; otherwise, Math.Floor is used and NO rounding takes place.</summary>
        public bool AllowRounding { get; set; } = true;

        /// <summary>Number of decimal places to use when rounding</summary>
        public int NumberOfDecimalPlaces { get; set; }

        /// <summary>The output format to use when converting a number into a string</summary>
        public string StringFormat { get; set; }


        /// <summary>Converts a double to a string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            double data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (double?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (double)value;
            }

            return string.IsNullOrWhiteSpace(StringFormat) ? data.ToString() : data.ToString(StringFormat);
        }

        /// <summary>Converts a string to a double</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return 0.0;
            }

            // NumberStyles.Float used to allow parse to deal with exponents
            if (double.TryParse(value, NumberStyles.Float, null, out double number))
            {
                if (AllowRounding && NumberOfDecimalPlaces > -1)
                {
                    number = Math.Round(number, NumberOfDecimalPlaces, Mode);
                }

                return number;
            }
            else if (value.IndexOf(",") > -1)
            {
                // There are commas in the value. Try removing them.
                var noComma = value.Replace(",", "");
                return GetReadData(inputType, noComma, columnName, columnIndex, rowNumber);
            }

            string convertSpecialStrings = ConvertSpecialStrings(value);
            // Avoid a recursive loop by comparing what was passed in to what the ConvertSpecialStrings
            // method returns. If they are the same, we need to exit and log an error!
            if (string.CompareOrdinal(convertSpecialStrings, value) == 0)
            {
                ThrowConvertErrorWhileReading(typeof(CsvConverterDefaultDouble),
                    inputType, value, columnName, columnIndex, rowNumber);
            }

            return GetReadData(inputType, convertSpecialStrings, columnName, columnIndex, rowNumber);

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
                    AllowRounding = settings.AllowRounding;
                    NumberOfDecimalPlaces = settings.NumberOfDecimalPlaces;
                    Mode = settings.Mode;
                }
            }
        }
    }
}
