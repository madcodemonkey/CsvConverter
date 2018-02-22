using CsvConverter;
using CsvConverter.CsvToClass;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerPreprocessor : ICsvToClassPreprocessor
    {
        private char _characterToAddToShortStrings;
        private int _maximumLength;
        private int _minimumLength;

        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPreProcessor;

        public bool CanProcessType(Type theType)
        {
            return theType == typeof(string);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var myAttribute = attribute as TextLengthEnforcerPreprocessorAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(TextLengthEnforcerPreprocessorAttribute)} attribute with this pre-processor!");

            _maximumLength = myAttribute.MaximumLength;
            _minimumLength = myAttribute.MinimumLength;
            _characterToAddToShortStrings = myAttribute.CharacterToAddToShortStrings;
        }

        public string Work(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (csvField != null)
            {
                if (csvField.Length < _minimumLength)
                {
                    while (csvField.Length < _minimumLength)
                        csvField += _characterToAddToShortStrings;

                }
                else if (csvField.Length > _maximumLength)
                {
                    csvField = csvField.Substring(0, _maximumLength);
                }
            }

            return csvField;
        }
    }
}
