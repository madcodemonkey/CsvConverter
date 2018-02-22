using CsvConverter;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverterAttribute : CsvConverterCustomAttribute
    {
        public MoneyFormatterClassToCsvTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }

        public string Format { get; set; } = "C";
    }
}
