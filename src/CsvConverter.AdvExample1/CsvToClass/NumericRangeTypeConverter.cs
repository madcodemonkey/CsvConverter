using System;
using CsvConverter;
using CsvConverter.CsvToClass;

namespace AdvExample1
{
    public class NumericRangeTypeConverter : ICsvToClassTypeConverter
    {
        private int _minimum = 0;
        private int _maximum = 20;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        public int Order => 999;

        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(int) || outputType == typeof(int?);
        }

        public object Convert(Type outputType, string stringValue, string columnName,
            int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            var data = defaultConverter.Convert(outputType, stringValue, columnName, columnIndex, rowNumber);
            if (data == null)
                return data;

            int dataAsNumber = (int)data;
            if (dataAsNumber < _minimum)
                dataAsNumber = _minimum;
            else if (dataAsNumber > _maximum)
                dataAsNumber = _maximum;


            return dataAsNumber;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            NumericRangeTypeConverterAttribute myAttribute = attribute as NumericRangeTypeConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(NumericRangeTypeConverterAttribute)} attribute with this converter!");

            _minimum = myAttribute.Minimum;
            _maximum = myAttribute.Maximum;
        }

    }
}
