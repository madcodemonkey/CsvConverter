using CsvConverter;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerConverterAttribute : CsvConverterCustomAttribute
    {
        public TextLengthEnforcerConverterAttribute() : base(typeof(TextLengthEnforcerCsvToClassPreConverter)) { }

        public char CharacterToAddToShortStrings { get; set; } = '~';
        public int MaximumLength { get; set; } = int.MaxValue;
        public int MinimumLength { get; set; } = int.MinValue;
    }
}
