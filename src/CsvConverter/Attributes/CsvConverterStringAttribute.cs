using System;

namespace CsvConverter
{
    /// <summary>Used for converting strings</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterStringAttribute : CsvConverterBaseAttribute
    {
        /// <summary>Default constructor.  Do NOT use this unless you have overriden GetConverter!</summary>
        public CsvConverterStringAttribute() { }

        /// <summary>Use this if you have not override GetCoverter and want the default code to run and create a 
        /// type converters.</summary>
        /// <param name="converterType"></param>
        public CsvConverterStringAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        public bool IsPreConverter { get; set; } = false;
        public bool IsPostConverter { get; set; } = false;

        /// <summary>An optional, order for pre and post converters in case there are more than one decorating a property or class.</summary>
        public int Order { get; set; } = 999;
    }
}
