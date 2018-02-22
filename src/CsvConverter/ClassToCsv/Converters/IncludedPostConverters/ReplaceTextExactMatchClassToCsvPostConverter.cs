using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>If the input string matches the old value exactly, it is replaced entirely with the new value.</summary>
    public class ReplaceTextExactMatchClassToCsvPostConverter : IClassToCsvPostConverter
    {
        private string _newValue;
        private string _oldValue;

        public int Order { get; set; } = 1;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvPost;

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var postProcess = attribute as CsvConverterOldAndNewValueAttribute;
            if (postProcess == null)
                throw new ArgumentException($"Please use the {nameof(CsvConverterOldAndNewValueAttribute)} attribute with this post converter ({nameof(ReplaceTextEveryMatchClassToCsvPostConverter)}).");

            _newValue = postProcess.NewValue;
            _oldValue = postProcess.OldValue;
        }

     

        public string Convert(string csvField, string columnName, int columnIndex, int rowNumber)
        {            
            if (csvField == _oldValue)
                return _newValue;

            return csvField;
        }
    }
}
