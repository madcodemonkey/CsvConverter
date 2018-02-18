using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectBooleanTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(bool) || outputType == typeof(bool?);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return false;
            }

            if (bool.TryParse(stringValue, out bool booleanValue))
            {
                return booleanValue;
            }

            var lower = stringValue.Trim().ToLower();
            if (lower == "y" || lower == "true" || lower == "t" || lower == "1")
            {
                return true;
            }

            if (lower == "n" || lower == "false" || lower == "f" || lower == "0")
            {
                return false;
            }

            ThrowCannotConvertError(targetType, stringValue, columnName, columnIndex, rowNumber);

            return false; // Never reached!
        }

        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }

}
