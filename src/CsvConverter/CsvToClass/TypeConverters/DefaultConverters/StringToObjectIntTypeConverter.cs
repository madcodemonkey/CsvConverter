using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectIntTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(int) || outputType == typeof(int?);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return 0;
            }

            if (int.TryParse(stringValue, out int number))
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
            return 0;
        }

        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }

}
