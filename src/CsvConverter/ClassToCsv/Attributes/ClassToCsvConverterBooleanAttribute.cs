using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.ClassToCsv
{
    /// <summary>A shortcut way of using the ObjectToStringBooleanTypeConverter</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ClassToCsvConverterBooleanAttribute : CsvConverterCustomAttribute
    {
        public ClassToCsvConverterBooleanAttribute(BooleanOutputFormatEnum outputFormat) : base(typeof(ObjectToStringBooleanTypeConverter))
        {
            OutputFormat = outputFormat;
        }

        /// <summary>Indicates how you would like booleans output to the CSV file.</summary>
        public BooleanOutputFormatEnum OutputFormat{ get; set; }
    }
}
