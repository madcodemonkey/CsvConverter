using System;

namespace CsvConverter
{
    /// <summary>Use this attribute when the converter needs and old and new value for a property.
    /// This primarily used for a direct substitution.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterStringTrimAttribute : CsvConverterStringAttribute
    {
        /// <summary>Constructor</summary>
        /// <param name="converterType"></param>
        public CsvConverterStringTrimAttribute(Type converterType) : base(converterType) { }

        /// <summary>Explains how you would like the string to be trimmed.</summary>
        public CsvConverterTrimEnum TrimAction { get; set; }
    }
}
