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

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IObjectToStringConverter defaultConverter)
        {
            if (value == null)
                return null;

            var theString =  value.ToString();

            return theString.Trim();
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {

        }
    }
}
