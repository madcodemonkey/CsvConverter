using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    /// <summary>Converts a string to a decimal or double value and then rounds it to the nearest integer.</summary>
    public class DecimalToIntCsvToClassConverter : ICsvToClassTypeConverter
    {
        private MidpointRounding _midpointRounding = MidpointRounding.AwayFromZero;
        private bool _allowRounding = true;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        public int Order => 999;

        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(int) || outputType == typeof(int?);
        }

        public object Convert(Type outputType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (outputType.HelpIsNullable())
                    return null;

                return 0;
            }

            var number = (decimal)defaultConverters.Convert(typeof(decimal), stringValue, columnName, columnIndex, rowNumber);
            number = _allowRounding ? Math.Round(number, 0, _midpointRounding) : Math.Floor(number);
            return (int)number;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            // Custom attribute is OPTIONAL
            var customAttribute = attribute as CsvConverterMathRoundingAttribute;
            if (customAttribute != null)
            {
                _allowRounding = customAttribute.AllowRounding;
                _midpointRounding = customAttribute.Mode;
            }
        }
    }
}
