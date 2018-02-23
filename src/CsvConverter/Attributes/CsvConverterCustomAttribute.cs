using System;

namespace CsvConverter
{
    /// <summary>This is the basic custom converter attribute and is used for CSV to Class and Class to CSV operations
    /// when custom converters are needed.  It can be used on a property for Type converters and on both a property
    /// and class for pre and post processing converters.  This is the basic attribute were the converters do NOT 
    /// require any special parameter inputs.  If you need special parameter inputs, you inherit from this attribute and add them.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterCustomAttribute : Attribute
    {
        public CsvConverterCustomAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        /// <summary>An optional, order to process the attribute in case there are more than one.</summary>
        public int Order { get; set; } = 999;

        /// <summary>The class that will convert data.</summary>       
        public Type ConverterType { get; set; }

        /// <summary>When this attribute is used to decorate a class, you can target a particular target type. 
        /// It is NOT used when decorating a property!</summary>
        public Type TargetPropertyType { get; set; }
    }
}
