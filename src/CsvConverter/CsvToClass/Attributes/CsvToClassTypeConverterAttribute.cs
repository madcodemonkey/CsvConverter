using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>When reading CSV files, this is optional type converter in case you want control how the string is converted.</summary>   

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsvToClassTypeConverterAttribute : Attribute
    {
        /// <summary>When reading CSV files, this is optional type converter in case you want control how the string is converted.</summary>   
        /// <param name="typeConverter">The class that will manipulate the CSV column.   Specify the type of a class that implements
        /// the ICsvToClassTypeConverter interface.</param>
        public CsvToClassTypeConverterAttribute(Type typeConverter)
        {
            TypeConverter = typeConverter;
        }

        /// <summary>The class that will manipulate the CSV column.  Specify the type of a class that implements the ICsvToClassTypeConverter interface.</summary>       
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
