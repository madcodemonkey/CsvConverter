using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringIntTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(int) || inputType == typeof(int?);
        }

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            int data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (int?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (int)value;
            }

            return data.ToString(stringFormat);
        }
        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
