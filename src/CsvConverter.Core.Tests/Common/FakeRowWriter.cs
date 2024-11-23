using CsvConverter.RowTools;

namespace CsvConverter.Core.Tests
{
    internal class FakeRowWriter : IRowWriter
    {
        public char EscapeChar { get; set; } = '"';
        public int RowNumber { get; set; } = 1;
        public char SplitChar { get; set; } = ',';

        public string WriteString { get; set; } = string.Empty;
        public List<List<string>> Rows { get; set; } = new();
        public List<string> LastRow { get; set; } = new();

        public void Write(List<string> fieldList)
        {
            LastRow = fieldList;
            Rows.Add(fieldList);
        }

        public void Write(string line)
        {
            WriteString = line;
        }
    }
}
