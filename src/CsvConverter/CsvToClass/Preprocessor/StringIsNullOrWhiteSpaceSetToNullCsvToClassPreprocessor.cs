using System;

namespace CsvConverter.CsvToClass
{
    public class StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor : ICsvToClassPreprocessor
    {
        public int Order { get; set; } = 999;

        public bool CanProcessType(Type theType)
        {
            return theType == typeof(string);
        }

        public void Initialize(CsvToClassPreprocessorAttribute attribute)
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
