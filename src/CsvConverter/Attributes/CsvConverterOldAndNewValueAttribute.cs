using System;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterOldAndNewValueAttribute : CsvConverterCustomAttribute
    {
        public CsvConverterOldAndNewValueAttribute(Type converterType) : base(converterType) { }

        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
