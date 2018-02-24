using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringDecimalTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(decimal) || inputType == typeof(decimal?);
        }

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName,
            int columnIndex, int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            decimal data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (decimal?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (decimal)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
