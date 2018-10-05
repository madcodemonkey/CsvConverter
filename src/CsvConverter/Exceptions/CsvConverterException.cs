using System;

namespace CsvConverter
{
    /// <summary>A generic exeception class for the CSV Converter project.</summary>
    public class CsvConverterException : Exception
    {
        /// <summary>Constructor</summary>
        /// <param name="message">Error message</param>
        public CsvConverterException(string message) : base(message)
        {
        }

        /// <summary>Constructor</summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public CsvConverterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
