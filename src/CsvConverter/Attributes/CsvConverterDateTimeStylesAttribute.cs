using System;
using System.Globalization;
using CsvConverter.TypeConverters;

namespace CsvConverter
{
    /// <summary>Used for specifying parsing formats for DateTime properties</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterDateTimeStylesAttribute : CsvConverterCustomAttribute, IDateConverterDetails
    {
         public CsvConverterDateTimeStylesAttribute(Type converterType) : base(converterType) { }

        /// <summary>The format to use with the DateTime ParseExact method.</summary>
        public string DateParseExactFormat { get; set; }

        /// <summary>The style to use with the DateTime ParseExact method.</summary>
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;
    }
}
