using System.Collections.Generic;

namespace CsvConverter.RowTools
{
    public interface IRowReader
    {
        bool IsRowBlank { get; }
        int LastColumnCount { get; }
        int RowNumber { get; }
        bool CanRead();
        List<string> ReadRow();
    }
}