using System;
using System.Globalization;
using CsvConverter.Shared;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectDateTimeTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter, IDateConverterSettings
    {
        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(DateTime) || outputType == typeof(DateTime?);
        }

         public string DateParseExactFormat { get; set; }
        public IFormatProvider DateFormatProvider { get; set; } = CultureInfo.InvariantCulture;
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return DateTime.MinValue;
            }

            if (string.IsNullOrWhiteSpace(DateParseExactFormat) == false &&
                DateTime.TryParseExact(stringValue, DateParseExactFormat, DateFormatProvider, DateStyle, out DateTime exactSomeDate))
            {
                return exactSomeDate;
            }
            else if (DateTime.TryParse(stringValue, out DateTime someDate))
            {
                return someDate;
            }
            else if (double.TryParse(stringValue, out var someDouble))
            {
                return DateTime.FromOADate(someDouble);
            }

            ThrowCannotConvertError(targetType, stringValue, columnName, columnIndex, rowNumber);
            return null;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            if (attribute != null)
            {
                var dateInteface = attribute as IDateConverterDetails;
                if (dateInteface != null)
                {
                    DateParseExactFormat = dateInteface.DateParseExactFormat;
                    DateStyle = dateInteface.DateStyle;
                }
            }
        }
    }

}
