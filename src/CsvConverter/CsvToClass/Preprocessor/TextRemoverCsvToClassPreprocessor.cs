using System;

namespace CsvConverter.CsvToClass
{
    public class TextRemoverCsvToClassPreprocessor : ICsvToClassPreprocessor
    {
        private string _textToRemover;
        public int Order { get; set; } = 999;

        public bool CanProcessType(Type theType)
        {
            return true;
        }

        public void Initialize(CsvToClassPreprocessorAttribute attribute)
        {
            Order = attribute.Order;
            _textToRemover = attribute.StringInput;
        }

        public string Work(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(csvField))
                return csvField;
            if (string.IsNullOrEmpty(_textToRemover))
                return csvField;

            return csvField.Replace(_textToRemover, "");
        }
    }
}
