using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>Turns a string that is a percentage into a decimal value or throws an exception if the conversion fails.</summary>
    public class CsvConverterPercentage : CsvConverterTypeBase, ICsvConverter
    {
        private ICsvConverter _decimalConverter;

        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(decimal) || propertyType == typeof(decimal?);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(decimal) || propertyType == typeof(decimal?);
        }

        /// <summary>Converts a percentage string into a decimal or decimal? value.</summary>
        public object GetReadData(Type targetType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return 0.0m;
            }

            // If there is a percentage sign attempt to handle it; 
            // otherwise, differ to the default converter.
            int indexOfPercentSign = value.IndexOf("%");
            if (indexOfPercentSign != -1)
            {
                // Remove the percentage sign
                value = value.Remove(indexOfPercentSign);
            }

            // Assign the decimal
            object someData = _decimalConverter.GetReadData(targetType, value, columnName, columnIndex, rowNumber);
            if (someData != null)
            {
                return (decimal)someData / 100.0m;
            }

            throw new ArgumentException($"The {nameof(CsvConverterPercentage)} converter cannot parse the string " +
                  $"'{value}' as a {targetType.Name} on row number {rowNumber} in " +
                  $"column {columnName} at column index {columnIndex}.");
        }

        /// <summary>Converts a decimal into a percentage string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            return _decimalConverter.GetWriteData(inputType, value, columnName, columnIndex, rowNumber);
        }

        public override void Initialize(CsvConverterBaseAttribute attribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (!(attribute is CsvConverterNumberAttribute oneAttribute))
            {
                throw new CsvConverterAttributeException(
                      $"Please use the {nameof(CsvConverterNumberAttribute)} " +
                      $"attribute with the {nameof(CsvConverterPercentage)} converter.");
            }

            if (string.IsNullOrWhiteSpace(oneAttribute.StringFormat))
            {
                int percession = oneAttribute.NumberOfDecimalPlaces / 2;
                oneAttribute.StringFormat = $"P{percession}";
            }

            _decimalConverter = defaultFactory.CreateConverter(typeof(decimal));
            _decimalConverter.Initialize(oneAttribute, defaultFactory);
        }
    }
}
