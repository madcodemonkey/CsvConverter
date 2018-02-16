using System;

namespace CsvConverter.ClassToCsv
{
    public class ClassToCsvReplaceTextPostprocessorAttribute : ClassToCsvPostProcessorAttribute
    {    
        public ClassToCsvReplaceTextPostprocessorAttribute(Type postProcessor, string oldValue, string newValue) : base(postProcessor)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
