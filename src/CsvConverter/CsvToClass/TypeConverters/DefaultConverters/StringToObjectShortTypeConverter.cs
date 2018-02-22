using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectShortTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(short) || outputType == typeof(short?);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return (short)0;
            }

            if (short.TryParse(stringValue, out short number))
            {
                return number;
            }
            else if (stringValue.IndexOf(",") > -1)
            {
                // There are commas in the value. Try removing them.
                var noComma = stringValue.Replace(",", "");
                return Convert(targetType, noComma, columnName, columnIndex, rowNumber, defaultConverter);
            }

            ThrowCannotConvertError(targetType, stringValue, columnName, columnIndex, rowNumber);
            return (short)0;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }

}
