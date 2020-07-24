using System;

namespace CsvConverter
{
    /// <summary>This converter replaces any null or white spaced string, with a new value.
    /// <see href="https://github.com/madcodemonkey/CsvConverter/wiki/Advanced-Converters-CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue">Documentation here.</see></summary>
    public class CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue : CsvConverterStringBase, ICsvConverterString
    {
        private string _newValue;

        /// <summary>Can this converter turn a CSV column string into the property type specified?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(string);
        }
        
        /// <summary>Converts a string into a another string if there is an exact match; otherwise, the
        /// original string is left untouched.</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            return Convert(value);
        }

        /// <summary>Converts a string into a another string if there is an exact match; otherwise, the
        /// original string is left untouched.</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            return Convert(value == null ? null : (string)value);
        }

        /// <summary>If a string is null or white space, the value is replaced with whatever is in _newValue.</summary>
        /// <param name="value">The value to examine and potentially replace.</param>
        private string Convert(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return _newValue;

            return value;
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (!(attribute is CsvConverterStringOldAndNewAttribute oneAttribute))
                throw new CsvConverterAttributeException(
                    $"Please use the {nameof(CsvConverterStringOldAndNewAttribute)} " +
                    $"attribute with the {nameof(CsvConverterStringReplaceTextExactMatch)} converter.");

            _newValue = oneAttribute.NewValue;
        }
    }
}
