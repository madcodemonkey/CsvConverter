using System;

namespace CsvConverter
{
    /// <summary>Replaces the text specified in the OldValue attribute with text in the NewValue attribute.</summary>
    public class CsvConverterStringReplaceTextExactMatch : CsvConverterStringBase, ICsvConverterString
    {
        private string _newValue;
        private string _oldValue;
        private bool _isCaseSensitive = true;

        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
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
           return Convert(value == null ? null : (string) value);
        }

        /// <summary>Performs operations on a CSV value BEFORE the type converter is called.
        /// In this case, text is replaced beforehand.</summary>
        /// <param name="value">Value straight from the CSV file before the TYPE converter touches it.</param>
        /// <param name="columnName">Name of the column being read.</param>
        /// <param name="columnIndex">Index of the column being read</param>
        /// <param name="rowNumber">Row number being read.</param>
        /// <returns></returns>
        private string Convert(string value)
        {
            if (_isCaseSensitive || value == null)
            {
                if (value == _oldValue)
                    return _newValue;
            }
            else
            {
                if (string.Compare(value, _oldValue, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return _newValue;
            }

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
            _oldValue = oneAttribute.OldValue;
            _isCaseSensitive = oneAttribute.IsCaseSensitive;
        }
    }

}
