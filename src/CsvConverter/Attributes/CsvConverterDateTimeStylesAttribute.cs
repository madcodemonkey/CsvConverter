using System;
using System.Globalization;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterDateTimeStylesAttribute : CsvConverterCustomAttribute
    {
        public CsvConverterDateTimeStylesAttribute(Type converterType) : base(converterType) { }
        public string DateParseExactFormat { get; set; }
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;
    }
}
