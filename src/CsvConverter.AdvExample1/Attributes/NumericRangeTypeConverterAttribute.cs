using System;
using CsvConverter;

namespace AdvExample1
{
    public class NumericRangeTypeConverterAttribute : CsvConverterCustomAttribute
    {
        public NumericRangeTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public int Minimum { get; set; } = 1;
        public int Maximum { get; set; } = 20;
    }
}
