using System;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectStringTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(string);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverter)
        {
            return stringValue;
        }

        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {
            
        }
    }

}
