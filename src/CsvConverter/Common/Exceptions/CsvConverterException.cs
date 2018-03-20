using System;

namespace CsvConverter
{
    public class CsvConverterException : Exception
    {
        public CsvConverterException(string message) : base(message)
        {
        }
        public CsvConverterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
