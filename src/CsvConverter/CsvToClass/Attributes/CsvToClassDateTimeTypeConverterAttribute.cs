using System;
using System.Globalization;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public class CsvToClassDateTimeTypeConverterAttribute : CsvToClassTypeConverterAttribute, IDateConverterDetails
    {
        public CsvToClassDateTimeTypeConverterAttribute() : base(typeof(StringToObjectDateTimeTypeConverter)) { }
        public CsvToClassDateTimeTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public string DateParseExactFormat { get; set; }
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;
    }
}
