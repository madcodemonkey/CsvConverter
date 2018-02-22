using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>All instances of the old value are replaced with the new value.</summary>
    public class ReplaceTextEveryMatchPostProcessor : IClassToCsvPostprocessor
    {
        private string _newValue;
        private string _oldValue;

        public int Order { get; set; } = 1;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvPostProcessor;

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var postProcess = attribute as CsvConverterOldAndNewValueAttribute;
            if (postProcess == null)
                throw new ArgumentException($"Please use the {nameof(CsvConverterOldAndNewValueAttribute)} attribute with this post processor ({nameof(ReplaceTextEveryMatchPostProcessor)}).");
            if (postProcess.OldValue == null)
                throw new ArgumentException($"The string replace method will NOT allow you to specify a null for the old value!  This is the value it is searching for and null will not be found.");
            if (postProcess.OldValue == null || postProcess.OldValue.Length == 0)
                throw new ArgumentException($"The string replace method will NOT allow you to specify a zero length string for the old value!  This is the value it is searching for and a zero length string will not be found.");

            _newValue = postProcess.NewValue;
            _oldValue = postProcess.OldValue;
        }

        public string Work(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (csvField == null)
                return csvField;

            return csvField.Replace(_oldValue, _newValue);
        }
    }
}
