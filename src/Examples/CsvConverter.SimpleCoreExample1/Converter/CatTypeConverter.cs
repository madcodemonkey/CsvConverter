using CsvConverter;
using System;

namespace SimpleCoreExample1
{
    public class CsvConverterCatType : CsvConverterTypeBase, ICsvConverter
    {
        private ICsvConverter _intConverter;

        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(int);
        }

        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(CatTypesEnum);
        }

        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
                return CatTypesEnum.Unknown;

            object result =_intConverter.GetReadData(inputType, value, columnName, columnIndex, rowNumber);

            if (result == null)
                return CatTypesEnum.Unknown;

            return (CatTypesEnum) result;
        }

        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            if (value == null)
                return $"{(int) CatTypesEnum.Unknown}";

            return $"{(int)value}";
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
          _intConverter = defaultFactory.CreateConverter(typeof(int));
        }
    }
}
