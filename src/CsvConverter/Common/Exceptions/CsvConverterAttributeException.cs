using System;

namespace CsvConverter
{
    public class CsvConverterAttributeException : Exception
    {
        public CsvConverterAttributeException(string message) : base(message)
        {
        }
    }
}
