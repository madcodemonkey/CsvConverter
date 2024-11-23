using CsvConverter.Reflection;
using System;

namespace CsvConverter
{
    /// <summary>Converts a string to a decimal or double value and then rounds it to the nearest integer.</summary>
    public class CsvConverterDecimalToInt : CsvConverterTypeBase, ICsvConverter
    {
        private CsvConverterNumberAttribute _oneAttribute;
        private ICsvConverter _readDecimalConverter;
        private ICsvConverter _writeDecimalConverter;

        /// <summary>Can this converter turn a CSV column string into the property type specified?</summary>
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

        /// <summary>Converts a string to an int</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return 0;
            }

            var number = (decimal)_readDecimalConverter.GetReadData(typeof(decimal), value, columnName, columnIndex, rowNumber);
            return (int)number;
        }

        /// <summary>Converts an int to a decimal string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            if (inputType.HelpIsNullable())
            {

                return _writeDecimalConverter.GetWriteData(typeof(decimal?),
                   value != null ? Convert.ToDecimal((int)value) : (decimal?)null,
                   columnName, columnIndex, rowNumber);

            }
            else
            {
                return _writeDecimalConverter.GetWriteData(typeof(decimal), Convert.ToDecimal((int)value), columnName, columnIndex, rowNumber);
            }
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (!(attribute is CsvConverterNumberAttribute oneAttribute))
            {
                throw new CsvConverterAttributeException(
                      $"Please use the {nameof(CsvConverterNumberAttribute)} " +
                      $"attribute with the {nameof(CsvConverterDecimalToInt)} converter.");
            }

            _oneAttribute = oneAttribute;
            _writeDecimalConverter = defaultFactory.CreateConverter(typeof(decimal));
            _writeDecimalConverter.Initialize(oneAttribute, defaultFactory);

            oneAttribute.NumberOfDecimalPlaces = 0;
            _readDecimalConverter = defaultFactory.CreateConverter(typeof(decimal));
            _readDecimalConverter.Initialize(oneAttribute, defaultFactory);
        }
    }
}