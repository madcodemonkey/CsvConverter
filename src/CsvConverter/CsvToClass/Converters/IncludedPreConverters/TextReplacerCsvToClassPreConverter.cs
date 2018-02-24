using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>Replaces the text specified in the OldValue attribute with text in the NewValue attribute.</summary>
    public class TextReplacerCsvToClassPreConverter : ICsvToClassPreConverter
    {
        private string _oldValue;
        private string _newValue;
        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPre;

        public bool CanConvert(Type theType)
        {
            return true;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
           var oneAttribute = attribute as CsvConverterOldAndNewValueAttribute;
            if (oneAttribute == null)
            {
               throw new ArgumentException($"Please use the {nameof(CsvConverterOldAndNewValueAttribute)} " +
                    $"attribute with the {nameof(TextReplacerCsvToClassPreConverter)} pre-converter");
            }
            
            Order = attribute.Order;
            _oldValue = oneAttribute.OldValue;
            _newValue = oneAttribute.NewValue;
        }


        public string Convert(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(csvField))
                return csvField;
            if (string.IsNullOrEmpty(_oldValue))
                return csvField;

            return csvField.Replace(_oldValue, _newValue);
        }
    }
}
