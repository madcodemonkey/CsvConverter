using System;
using System.Globalization;
using CsvConverter.Shared;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectDoubleTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter, IDoubleConverterSettings
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(double) || outputType == typeof(double?);
        }
        
        public int NumberOfDecimalPlaces { get; set; } = -1;  // Not specified

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return 0.0;
            }

            // NumberStyles.Float used to allow parse to deal with exponents
            if (double.TryParse(stringValue, NumberStyles.Float, null, out double number))
            {
                if (NumberOfDecimalPlaces > -1)
                {
                    number = Math.Round(number, NumberOfDecimalPlaces);
                }

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

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            if (attribute != null)
            {
                var numberInterface = attribute as IDoubleConverterSettings;
                if (numberInterface != null)
                    NumberOfDecimalPlaces = numberInterface.NumberOfDecimalPlaces;
            }
        }
    }
}
