using System;

namespace CsvConverter.TypeConverters
{
    public interface IDateConverterSettings : IDateConverterDetails
    {
        IFormatProvider DateFormatProvider { get; set; } 
    }

}
