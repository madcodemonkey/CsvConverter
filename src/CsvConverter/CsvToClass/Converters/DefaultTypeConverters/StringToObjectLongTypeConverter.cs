using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectLongTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(long) || outputType == typeof(long?);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return (long)0;
            }

            if (long.TryParse(stringValue, out long number))
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
            return (long)0;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }

}
