using CsvConverter;
using CsvConverter.CsvToClass;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerCsvToClassPreConverter : ICsvToClassPreConverter
    {
        private char _characterToAddToShortStrings;
        private int _maximumLength;
        private int _minimumLength;

        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPre;

        public bool CanConvert(Type theType)
        {
            return theType == typeof(string);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var myAttribute = attribute as TextLengthEnforcerConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(TextLengthEnforcerConverterAttribute)} attribute with this pre-converter!");

            _maximumLength = myAttribute.MaximumLength;
            _minimumLength = myAttribute.MinimumLength;
            _characterToAddToShortStrings = myAttribute.CharacterToAddToShortStrings;
        }

        public string Convert(string csvField, string columnName, int columnIndex, int rowNumber)
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
