using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringShortTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(short) || inputType == typeof(short?);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            short data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (short?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (short)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {
        }
    }
}
