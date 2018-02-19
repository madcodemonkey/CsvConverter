using CsvConverter.ClassToCsv;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverterAttribute : ClassToCsvTypeConverterAttribute
    {
        public MoneyFormatterClassToCsvTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }

        public string Format { get; set; } = "C";
    }
}
