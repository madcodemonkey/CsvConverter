using System;
using System.Collections.Generic;
using System.Globalization;
using CsvConverter;
using CsvConverter.Reflection;

namespace AdvExample1
{
    public class CsvConverterMoney : CsvConverterTypeBase, ICsvConverter
    {
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(decimal) || propertyType == typeof(decimal?) ||
              propertyType == typeof(double) || propertyType == typeof(double?);
        }

        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(decimal) || propertyType == typeof(decimal?) ||
               propertyType == typeof(double) || propertyType == typeof(double?);
        }

        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (inputType.HelpIsNullable())
                    return null;

                return 0;
            }

            if (inputType == typeof(double) || inputType == typeof(double?))
                return double.Parse(value, NumberStyles.Currency);

            return decimal.Parse(value, NumberStyles.Currency);
        }

        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            if (_converterDictionary.ContainsKey(inputType) == false)
                throw new CsvConverterException($"The {nameof(CsvConverterMoney)} converter cannot write type {nameof(inputType)}");

            return _converterDictionary[inputType].GetWriteData(inputType, value, columnName, columnIndex, rowNumber);           
        }

        public override void Initialize(CsvConverterAttribute attribute, 
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);
            if (!(attribute is CsvConverterNumberAttribute oneAttribute))
            {
                throw new CsvConverterAttributeException(
                      $"Please use the {nameof(CsvConverterNumberAttribute)} " +
                      $"attribute with the {nameof(CsvConverterMoney)} converter.");
            }

            CreateConverter(typeof(decimal), oneAttribute, defaultFactory);
            CreateConverter(typeof(decimal?), oneAttribute, defaultFactory);
            CreateConverter(typeof(double), oneAttribute, defaultFactory);
            CreateConverter(typeof(double?), oneAttribute, defaultFactory);
        }

        private Dictionary<Type, ICsvConverter> _converterDictionary = new Dictionary<Type, ICsvConverter>();
        private void CreateConverter(Type inputType, 
            CsvConverterNumberAttribute oneAttribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            ICsvConverter converter = defaultFactory.CreateConverter(inputType);
            converter.Initialize(oneAttribute, defaultFactory);
            _converterDictionary.Add(inputType, converter);

        }

    }
}
