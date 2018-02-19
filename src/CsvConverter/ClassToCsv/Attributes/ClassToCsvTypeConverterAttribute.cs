using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>When writing CSV files, this is optional type converter in case you want control how the string is written to the CSV file.</summary>   

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ClassToCsvTypeConverterAttribute : Attribute
    {
        /// <summary>When writing CSV files, this is optional type converter in case you want control how the string is written to the CSV file.</summary>   
        /// <param name="typeConverter">The class that will manipulate the class property into a string for the CSV field.   
        /// Specify the type of a class that implements the IClassToCsvTypeConverter interface.</param>
        public ClassToCsvTypeConverterAttribute(Type typeConverter)
        {
            TypeConverter = typeConverter;
        }

        /// <summary>The class that will manipulate the class property into a string for the CSV field.   
        /// Specify the type of a class that implements the IClassToCsvTypeConverter interface.</summary>       
        public Type TypeConverter { get; set; }

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
