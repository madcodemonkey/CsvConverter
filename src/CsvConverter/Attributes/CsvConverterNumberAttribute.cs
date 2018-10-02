using System;

namespace CsvConverter
{
    /// <summary>Used when converting numbers (int, decimal, double, float, byte, etc.)</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterNumberAttribute : CsvConverterBaseAttribute
    {
        /// <summary>Constructor used when you want the default to use the default/built in converters</summary>
        public CsvConverterNumberAttribute() : base() { }

        /// <summary>Constructor to use when you want to provide your own converter.</summary>
        /// <param name="converterType">A cstom converter</param>
        public CsvConverterNumberAttribute(Type converterType) : base(converterType) { }

        /// <summary>The output format to use when converting a number into a string</summary>
        public string StringFormat { get; set; }

        /// <summary>Mode is used when AllowRounding is true.  It is used with the Math.Round function.</summary>
        public MidpointRounding Mode { get; set; } = MidpointRounding.AwayFromZero;

        /// <summary>Default is TRUE and if true, Mode is used with Math.Round; otherwise, Math.Floor is used and NO rounding takes place.</summary>
        public bool AllowRounding { get; set; } = true;

        /// <summary>Number of decimal places to use when rounding</summary>
        public int NumberOfDecimalPlaces { get; set; }
    }


}
