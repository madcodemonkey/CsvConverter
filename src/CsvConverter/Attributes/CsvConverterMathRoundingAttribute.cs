using System;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterMathRoundingAttribute : CsvConverterCustomAttribute
    {
        public CsvConverterMathRoundingAttribute(Type typeConverter) : base(typeConverter) { }

        /// <summary>Mode is used when AllowRounding is true.  It is used with the Math.Round function.</summary>
        public MidpointRounding Mode { get; set; } = MidpointRounding.AwayFromZero;

        /// <summary>Default is TRUE and if true, Mode is used with Math.Round; otherwise, Math.Floor is used and NO rounding takes place.</summary>
        public bool AllowRounding { get; set; } = true;

        /// <summary>Number of decimal places to use when rounding</summary>
        public int NumberOfDecimalPlaces { get; set; } 
    }
}
