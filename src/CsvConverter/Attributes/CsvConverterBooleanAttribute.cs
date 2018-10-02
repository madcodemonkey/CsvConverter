using System;

namespace CsvConverter
{
    /// <summary>Used for converting booleans</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterBooleanAttribute : CsvConverterBaseAttribute
    {
        /// <summary>Constructor used when you want the default to use the default/built in converters</summary>
        public CsvConverterBooleanAttribute() : base() { }

        /// <summary>Constructor to use when you want to provide your own converter.</summary>
        /// <param name="converterType">A cstom converter</param>
        public CsvConverterBooleanAttribute(Type converterType) : base(converterType) { }

        /// <summary>Indicates the text that represents a true value in the CSV file (it is case insensitive for inputs and written as is for outputs)</summary>
        public string TrueValue { get; set; }

        /// <summary>Indicates the text that represents a false value in the CSV file (it is case insensitive for inputs and written as is for outputs)</summary>
        public string FalseValue { get; set; }
    }
}
