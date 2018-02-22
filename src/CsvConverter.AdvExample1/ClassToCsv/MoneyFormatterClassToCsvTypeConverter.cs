using CsvConverter;
using CsvConverter.ClassToCsv;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverter: IClassToCsvTypeConverter
    {
        private string _formatString;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order => 999;

        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(decimal) || inputType == typeof(decimal?) ||
                inputType == typeof(double) || inputType == typeof(double);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            if (value == null)
                return null;

            if (inputType == typeof(double))
                return ((double)value).ToString(_formatString);

            return ((decimal)value).ToString(_formatString);            
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            MoneyFormatterConverterAttribute myAttribute = attribute as MoneyFormatterConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(MoneyFormatterConverterAttribute)} attribute with this converter!");

            _formatString = myAttribute.Format;
        }
    }
}
