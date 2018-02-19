using System;
using CsvConverter.CsvToClass;

namespace AdvExample1
{
    public class NumericRangeTypeConverterAttribute : CsvToClassTypeConverterAttribute
    {
        public NumericRangeTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public int Minimum { get; set; } = 1;
        public int Maximum { get; set; } = 20;
    }
}
