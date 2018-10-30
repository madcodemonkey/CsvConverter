using System;
using CsvConverter;

namespace AdvExample3
{
    public class CsvConverterNumericRangeAttribute : CsvConverterAttribute
    {
        public CsvConverterNumericRangeAttribute(Type typeConverter) : base(typeConverter) { }
        public int Minimum { get; set; } = 1;
        public int Maximum { get; set; } = 20;
    }
}
