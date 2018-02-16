using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>Preprocess CSV column information.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvToClassPreprocessorAttribute: Attribute
    {
        /// <summary>Preprocess CSV column information.</summary>
        /// <param name="preprocessor">The class that will manipulate the CSV column.  Specify the type of a class that implements the 
        /// ICsvToClassPreprocessor interface.</param>
        public CsvToClassPreprocessorAttribute(Type preprocessor)
        {
            Preprocessor = preprocessor;
        }

        /// <summary>An optional, order to process the attribute in case order is important and there is more than one preprocessor.</summary>
        public int Order { get; set; } = 999;

        /// <summary>The class that will manipulate the CSV column.  Specify the type of a class that implements the ICsvToClassPreprocessor interface.</summary>       
        public Type Preprocessor { get; set; }
        
        /// <summary>When this attribute is used to decorate a class, you can target a particular target type.  If you don't specify 
        /// a target property type at the class level, it will be assumed that you want to target ALL properties assuming that
        /// CanProcessType on the preprossor returns true for the property type.  It is NOT used when decorating a property!</summary>
        public Type TargetPropertyType { get; set; }

        /// <summary>Optional data input.</summary>
        public int IntInput { get; set; }

        /// <summary>Optional data input.</summary>
        public int DoubleInput { get; set; }
        
        /// <summary>Optional data input.</summary>
        public int DecimalInput { get; set; }
        
        /// <summary>Optional data input.</summary>
        public string StringInput { get; set; }
    }
}
