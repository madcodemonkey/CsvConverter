using System;
using System.Globalization;

namespace CsvConverter.TypeConverters
{
    public class DateConverterSettings : IDateConverterSettings
    {
        public IFormatProvider DateFormatProvider { get; set; } = CultureInfo.InvariantCulture;
        public string DateParseExactFormat { get; set; }
        public DateTimeStyles DateStyle { get; set; } = DateTimeStyles.None;
    }

}
