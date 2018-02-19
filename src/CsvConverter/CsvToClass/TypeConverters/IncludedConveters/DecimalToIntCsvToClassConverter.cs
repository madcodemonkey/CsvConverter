using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    /// <summary>Converts a string to a decimal value and the rounds it to the nearest integer.</summary>
    public class DecimalToIntCsvToClassConverter : ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(int) || outputType == typeof(int?);
        }

        public object Convert(Type outputType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (outputType.HelpIsNullable())
                    return null;

                return 0;
            }

            var number = (decimal)defaultConverters.Convert(typeof(decimal), stringValue, columnName, columnIndex, rowNumber);
            number = Math.Round(number, 0, MidpointRounding.AwayFromZero);
            return (int)number;
        }

        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {

        }
    }
}
