using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>If the CSV field contains a null OR blank text, it changes it into a null.</summary>
    public class StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor : ICsvToClassPreprocessor
    {
        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPreProcessor;

        public bool CanProcessType(Type theType)
        {
            return theType == typeof(string);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            Order = attribute.Order;
        }

        public string Work(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(csvField))
                return null;

            return csvField;
        }
    }
}
