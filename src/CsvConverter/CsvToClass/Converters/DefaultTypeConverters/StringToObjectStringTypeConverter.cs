using System;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectStringTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(string);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            return stringValue;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            
        }
    }

}
