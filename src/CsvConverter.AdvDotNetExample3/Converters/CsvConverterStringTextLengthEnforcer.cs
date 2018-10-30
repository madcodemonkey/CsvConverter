using CsvConverter;
using System;

namespace AdvExample3
{
    public class CsvConverterStringTextLengthEnforcer : CsvConverterStringBase, ICsvConverterString
    {
        private char _characterToAddToShortStrings;
        private int _maximumLength;
        private int _minimumLength;

        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            if (value == null)
                return null;

            return EnforceMinimumLength((string)value);
        }

        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            return EnforceMinimumLength(value);
        }

        private string EnforceMinimumLength(string value)
        {
            if (value != null)
            {
                if (value.Length < _minimumLength)
                {
                    while (value.Length < _minimumLength)
                        value += _characterToAddToShortStrings;

                }
                else if (value.Length > _maximumLength)
                {
                    value = value.Substring(0, _maximumLength);
                }
            }

            return value;
        }
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            var myAttribute = attribute as CsvConverterTextLengthEnforcerAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(CsvConverterTextLengthEnforcerAttribute)} attribute with this pre-converter!");

            _maximumLength = myAttribute.MaximumLength;
            _minimumLength = myAttribute.MinimumLength;
            _characterToAddToShortStrings = myAttribute.CharacterToAddToShortStrings;
        }

    }
}
