using System;
using System.Globalization;

namespace CsvConverter.TypeConverters
{
    public interface IDateConverterSettings : IDateConverterDetails
    {
        IFormatProvider DateFormatProvider { get; set; } 
    }

    public interface IDateConverterDetails
    {
        string DateParseExactFormat { get; set; }
        DateTimeStyles DateStyle { get; set; } 

    }

}
