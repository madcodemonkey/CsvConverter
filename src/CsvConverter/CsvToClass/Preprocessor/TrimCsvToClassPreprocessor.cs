using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>Trims all fields of white space left and right of the text.</summary>
    public class TrimCsvToClassPreprocessor : ICsvToClassPreprocessor
    {
        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPreProcessor;

        public bool CanProcessType(Type theType)
        {
            return true;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            Order = attribute.Order;
        }

        public string Work(string csvField, string columnName, int columnIndex, int rowNumber)
        {            
            if (string.IsNullOrEmpty(csvField))
                return csvField;

            return csvField.Trim();
        }
    }
}
