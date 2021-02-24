using System.Collections.Generic;

namespace CsvConverter.RowTools
{
    /// <summary>Used for reading rows from a stream.</summary>
    public interface IRowReader
    {
        /// <summary>The Escape character needed if the SplitChar is found in the actual value in a column.  So, this is a comma separated file
        /// and the data contains a comma, we should escape the data with a double quote (someData, "someData,WithComman", other data)</summary>
        char EscapeChar { get; set; }

        /// <summary>Indicates if the row that was read is blank (meaning that it was NOTHING but commas with NO spaces)</summary>
        bool IsRowBlank { get; }

        /// <summary>Indicates the number of columns that were read the last time we read a row.  It always starts a zero when first initialized,
        /// but the first row read should give you a number greater than zero.  If the number changes from row to row, it is an indication that 
        /// the columns are incorrectly formatted (e.g., text with commas is NOT surrounded by quotes and causing the column count to increase)</summary>
        int LastColumnCount { get; }

        /// <summary>The current row being read.</summary>
        int RowNumber { get; }

        /// <summary>The character that delimits the data.</summary>
        char SplitChar  { get; set; }

        /// <summary>Indicates if the stream can be read.  In other words, is there more data in the file.</summary>
        bool CanRead();

        /// <summary>Reads one row and returns each column in a List of string</summary>
        List<string> ReadRow();
    }
}
