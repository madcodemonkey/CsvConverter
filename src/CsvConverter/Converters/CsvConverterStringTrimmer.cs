using System;

namespace CsvConverter
{
    /// <summary>Trims all fields of white space left and right of the text.</summary>
    public class CsvConverterStringTrimmer : CsvConverterStringBase, ICsvConverterString
    {
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

        public CsvConverterTrimEnum TrimAction { get; set; } = CsvConverterTrimEnum.All;

        public object GetReadData(Type targetType, string value, string columnName, int columnIndex, int rowNumber)
        {
            return TrimTheString(value);
        }

        /// <summary>Converts a string into a trimmed string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            return TrimTheString(value);
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterBaseAttribute attribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (attribute is CsvConverterStringTrimAttribute oneAttribute)
            {
                TrimAction = oneAttribute.TrimAction;
            }
        }

        private string TrimTheString(object value)
        {
            if (value == null)
                return null;

            var theString = value.ToString();

            switch (TrimAction)
            {
                case CsvConverterTrimEnum.TrimStart:
                    return theString.TrimStart();
                case CsvConverterTrimEnum.TrimEnd:
                    return theString.TrimEnd();
                default:
                    return theString.Trim();
            }
        }
    }

}
