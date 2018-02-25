using System.Collections.Generic;
using CsvConverter.RowTools;


namespace CsvConverter.Tests.Services
{
    internal class FakeRowWriter : IRowWriter
    {
        public int RowNumber { get; set; } = 1;

        public string WriteString { get; set; }
        public List<List<string>> Rows { get; set; } = new List<List<string>>();
        public List<string> LastRow { get; set; }

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
