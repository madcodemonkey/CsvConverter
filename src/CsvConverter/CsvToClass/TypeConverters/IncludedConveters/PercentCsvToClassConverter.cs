using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    /// <summary>Turns a string that is a percentage into a decimal value or throws an exception if the conversion fails.</summary>
    public class PercentCsvToClassConverter : ICsvToClassTypeConverter
    {
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassConverter;

        public int Order => 999;

        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(decimal) || outputType == typeof(decimal?);
        }
        
        public object Convert(Type outputType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (outputType.HelpIsNullable())
                    return null;

                return 0.0m;
            }

            // If there is a percentage sign attempt to handle it; 
            // otherwise, differ to the default converter.
            int indexOfPercentSign = stringValue.IndexOf("%");
            if (indexOfPercentSign != -1)
            {
                // Remove the percentage sign
                stringValue = stringValue.Remove(indexOfPercentSign);
            }

            // Assign the decimal
            object someData = defaultConverters.Convert(outputType, stringValue, columnName, columnIndex, rowNumber);
            if (someData != null)
            {
                return (decimal)someData / 100.0m;
            }

            throw new ArgumentException($"The {nameof(PercentCsvToClassConverter)} converter cannot parse the string " +
                  $"'{stringValue}' as a {outputType.Name} on row number {rowNumber} in " +
                  $"column {columnName} at column index {columnIndex}.");
        }


        public void Initialize(CsvConverterCustomAttribute attribute)
        {

        }
    }
}
