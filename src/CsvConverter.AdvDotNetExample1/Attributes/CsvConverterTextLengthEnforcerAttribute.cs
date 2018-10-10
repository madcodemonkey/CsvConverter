using CsvConverter;

namespace AdvExample1
{
    public class CsvConverterTextLengthEnforcerAttribute : CsvConverterStringAttribute
    {
        public CsvConverterTextLengthEnforcerAttribute() : base(typeof(CsvConverterStringTextLengthEnforcer)) { }

        public char CharacterToAddToShortStrings { get; set; } = '~';
        public int MaximumLength { get; set; } = int.MaxValue;
        public int MinimumLength { get; set; } = int.MinValue;
    }
}
