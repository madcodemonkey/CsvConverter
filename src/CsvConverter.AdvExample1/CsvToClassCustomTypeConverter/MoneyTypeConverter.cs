using System;
using System.Globalization;
using CsvConverter;
using CsvConverter.CsvToClass;
using CsvConverter.Shared;

namespace AdvExample1
{
    public class MoneyTypeConverter : ICsvToClassTypeConverter
    {
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        public int Order => 999;

        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(decimal) || outputType == typeof(decimal?) ||
            outputType == typeof(double) || outputType == typeof(double);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return 0;
            }

            if (targetType == typeof(double) || targetType == typeof(double?))
               return double.Parse(stringValue, NumberStyles.Currency);

            return decimal.Parse(stringValue, NumberStyles.Currency);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
          
        }

    }
}
