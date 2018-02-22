using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringFloatTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(float) || inputType == typeof(float?);
        }
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvConverter;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            float data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (float?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (float)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
