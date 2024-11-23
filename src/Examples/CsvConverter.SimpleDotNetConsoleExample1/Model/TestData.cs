using CsvConverter;

namespace SimpleDotNetConsoleExample1;

public class TestData
{
    [CsvConverter(ColumnIndex = 1)]
    public string FieldName { get; set; }

    [CsvConverter(ColumnIndex = 2)]
    public string FieldDescription { get; set; }

    [CsvConverter(ColumnIndex = 3)]
    public string StartPosition { get; set; }

    [CsvConverter(ColumnIndex = 4)]
    public string EndPosition { get; set; }
}