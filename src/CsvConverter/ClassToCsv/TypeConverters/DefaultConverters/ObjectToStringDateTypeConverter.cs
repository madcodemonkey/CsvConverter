using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringDateTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(DateTime) || inputType == typeof(DateTime?);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            DateTime data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (DateTime?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (DateTime)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {
        }
    }
}
