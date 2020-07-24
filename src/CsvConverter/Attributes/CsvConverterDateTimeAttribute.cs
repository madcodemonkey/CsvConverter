using System;
using System.Globalization;

namespace CsvConverter
{
    /// <summary>Used for converting DateTime</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterDateTimeAttribute : CsvConverterAttribute
    {
        /// <summary>Constructor used when you want the default to use the default/built in converters</summary>
        public CsvConverterDateTimeAttribute() : base() { }

        /// <summary>Constructor to use when you want to provide your own converter.</summary>
        /// <param name="converterType">A custom converter</param>
        public CsvConverterDateTimeAttribute(Type converterType) : base(converterType) { }

        /// <summary>The style to use with the DateTime ParseExact method when converting strings to dates.</summary>
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;

        /// <summary>The output format to use when converting a DateTime into a string OR 
        /// The format to use with the DateTime ParseExact method when converting strings to dates.</summary>
        public string StringFormat { get; set; }
    }

}
