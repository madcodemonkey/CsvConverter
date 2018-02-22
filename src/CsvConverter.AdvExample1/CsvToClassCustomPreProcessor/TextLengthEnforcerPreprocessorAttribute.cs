using CsvConverter;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerPreConverterAttribute : CsvConverterCustomAttribute
    {
        public TextLengthEnforcerPreConverterAttribute() : base(typeof(TextLengthEnforcerPreConverter)) { }

        public char CharacterToAddToShortStrings { get; set; } = '~';
        public int MaximumLength { get; set; } = int.MaxValue;
        public int MinimumLength { get; set; } = int.MinValue;
    }
}
