using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>Preprocess CSV column information.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ClassToCsvPostProcessorAttribute : Attribute
    {
        /// <summary>Postprocess CSV column information.</summary>
        /// <param name="postProcessor">The class that will manipulate text that will be written into a CSV column after the converter transformed it into a string. 
        /// Specify the type of a class that implements the IClassToCsvPostprocessor interface.</param>
        public ClassToCsvPostProcessorAttribute(Type postProcessor)
        {
            Postprocessor = postProcessor;
        }

        /// <summary>An optional, order to process the attribute in case order is important and there is more than one preprocessor.</summary>
        public int Order { get; set; } = 999;

        /// <summary>The class that will manipulate the CSV column.  Specify the type of a class that implements the IClassToCsvPostprocessor interface.</summary>       
        public Type Postprocessor { get; set; }
        
        /// <summary>When this attribute is used to decorate a class, you can target a particular target type.  If you don't specify 
        /// a target property type at the class level, it will be assumed that you want to target ALL properties assuming that
        /// CanProcessType on the preprossor returns true for the property type.  It is NOT used when decorating a property!</summary>
        public Type TargetPropertyType { get; set; }    
    }
}
