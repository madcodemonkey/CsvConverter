using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringLongTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(long) || inputType == typeof(long?);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            long data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (long?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (long)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {
        }
    }
}
