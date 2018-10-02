using System;

namespace CsvConverter
{
    /// <summary>A CSV Converter attribute exception</summary>
    public class CsvConverterAttributeException : Exception
    {
        /// <summary>Constructor.</summary>
        /// <param name="message">Error message</param>
        public CsvConverterAttributeException(string message) : base(message)
        {
        }
    }
}
