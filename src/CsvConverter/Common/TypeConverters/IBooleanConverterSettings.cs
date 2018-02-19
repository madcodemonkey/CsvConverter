using CsvConverter.ClassToCsv;

namespace CsvConverter.TypeConverters
{
    public interface IBooleanConverterSettings
    {
        BooleanOutputFormatEnum OutputFormat { get; set; }
    }
}