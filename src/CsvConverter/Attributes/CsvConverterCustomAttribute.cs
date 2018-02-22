using System;

namespace CsvConverter
{
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
