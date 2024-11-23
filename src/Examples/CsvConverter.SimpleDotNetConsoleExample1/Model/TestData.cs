using CsvConverter;

namespace SimpleDotNetConsoleExample1;

public class TestData
{
    [CsvConverter(ColumnIndex = 1)]
    public string FieldName { get; set; } = string.Empty;

    [CsvConverter(ColumnIndex = 2)]
    public string FieldDescription { get; set; } = string.Empty;

    [CsvConverter(ColumnIndex = 3)]
    public string StartPosition { get; set; } = string.Empty;

    [CsvConverter(ColumnIndex = 4)]
    public string EndPosition { get; set; } = string.Empty;
}