using System;
using System.Globalization;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectFloatTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(float) || outputType == typeof(float?); ;
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return (float)0;
            }
            
            // NumberStyles.Float used to allow parse to deal with exponents
            if (float.TryParse(stringValue, NumberStyles.Float, null, out float number))
            {
                return number;
            }
            else if (stringValue.IndexOf(",") > -1)
            {
                // There are commas in the value. Try removing them.
                var noComma = stringValue.Replace(",", "");
                return Convert(targetType, noComma, columnName, columnIndex, rowNumber, defaultConverter);
            }

            string convertSpecialStrings = ConvertSpecialStrings(stringValue);
            // Avoid a recursive loop by comparing what was passed in to what the ConvertSpecialStrings
            // method returns. If they are the same, we need to exit and log an error!
            if (string.CompareOrdinal(convertSpecialStrings, stringValue) == 0)
            {
                ThrowCannotConvertError(targetType, stringValue, columnName, columnIndex, rowNumber);
            }

            return Convert(targetType, convertSpecialStrings, columnName, columnIndex, rowNumber, defaultConverter);
        }

        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }

}
