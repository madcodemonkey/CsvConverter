using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>Trims all fields of white space left and right of the text.</summary>
    public class TrimClassToCsvTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(string);
        }
 
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            if (value == null)
                return null;

            var theString =  value.ToString();

            return theString.Trim();
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
