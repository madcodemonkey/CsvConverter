using System.Globalization;

namespace CsvConverter.TypeConverters
{
    public interface IDateConverterDetails
    {
        string DateParseExactFormat { get; set; }
        DateTimeStyles DateStyle { get; set; } 

    }

}
