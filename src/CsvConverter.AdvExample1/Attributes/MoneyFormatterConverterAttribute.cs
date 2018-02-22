using CsvConverter;
using System;

namespace AdvExample1
{
    public class MoneyFormatterConverterAttribute : CsvConverterCustomAttribute
    {
        public MoneyFormatterConverterAttribute(Type typeConverter) : base(typeConverter) { }

        public string Format { get; set; } = "C";
    }
}
