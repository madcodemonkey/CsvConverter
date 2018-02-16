using System.Collections.Generic;

namespace CsvConverter.RowTools
{
    public interface IRowWriter
    {
        int RowNumber { get; }
        void Write(List<string> fieldList);
        void Write(string line);
    }
}