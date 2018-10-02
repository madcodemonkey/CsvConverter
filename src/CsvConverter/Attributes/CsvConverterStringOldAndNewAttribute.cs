using System;

namespace CsvConverter
{
    /// <summary>Use this attribute when the converter needs and old and new value for a property.
    /// This primarily used for a direct substitution.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterStringOldAndNewAttribute : CsvConverterStringAttribute
    {
        /// <summary>Constructor</summary>
        /// <param name="converterType"></param>
        public CsvConverterStringOldAndNewAttribute(Type converterType) : base(converterType) { }

        /// <summary>Old value</summary>
        public string OldValue { get; set; }

        /// <summary>New Value</summary>
        public string NewValue { get; set; }
    }
}
