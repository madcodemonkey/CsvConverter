using CsvConverter;
using System;

namespace AdvExample3
{
    public class CsvConverterNumericRange : CsvConverterTypeBase, ICsvConverter
    {
        private int _minimum = 0;
        private int _maximum = 20;

        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(int) || propertyType == typeof(int?);
        }

        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(int) || propertyType == typeof(int?);
        }

        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            int valueAsInt = (value == null) ? 0 :
              (int)_intConverter.GetReadData(inputType, value, columnName, columnIndex, rowNumber);

            return EnforceMinMaxLength(valueAsInt);
        }



        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            int valueAsInt = EnforceMinMaxLength(value == null ? 0 : (int)value);

            return _intConverter.GetWriteData(inputType, valueAsInt, columnName, columnIndex, rowNumber);
        }

        public override void Initialize(CsvConverterAttribute attribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (!(attribute is CsvConverterNumericRangeAttribute oneAttribute))
                throw new ArgumentException($"Please use the {nameof(CsvConverterNumericRangeAttribute)} attribute with this converter!");

            _intConverter = defaultFactory.CreateConverter(typeof(int));
            _intConverter.Initialize(oneAttribute, defaultFactory);

            _minimum = oneAttribute.Minimum;
            _maximum = oneAttribute.Maximum;
        }

        private ICsvConverter _intConverter;
        private int EnforceMinMaxLength(int valueAsInt)
        {
            if (valueAsInt < _minimum)
                valueAsInt = _minimum;
            else if (valueAsInt > _maximum)
                valueAsInt = _maximum;
            return valueAsInt;
        }
    }
}
