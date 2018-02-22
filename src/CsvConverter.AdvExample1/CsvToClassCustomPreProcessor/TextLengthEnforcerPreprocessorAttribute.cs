using CsvConverter;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerPreprocessorAttribute : CsvConverterCustomAttribute
    {
        public TextLengthEnforcerPreprocessorAttribute() : base(typeof(TextLengthEnforcerPreprocessor)) { }

        public char CharacterToAddToShortStrings { get; set; } = '~';
        public int MaximumLength { get; set; } = int.MaxValue;
        public int MinimumLength { get; set; } = int.MinValue;
    }
}
