using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringDoubleTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(double) || inputType == typeof(double?);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName,
            int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            double data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (double?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (double)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {

        }
    }
}
