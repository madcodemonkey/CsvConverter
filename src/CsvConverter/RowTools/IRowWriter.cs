using System.Collections.Generic;

namespace CsvConverter.RowTools
{
    /// <summary>Used for writing rows to a stream.</summary>
    public interface IRowWriter
    {
        /// <summary>The Escape character needed if the SplitChar is found in the actual value in a column.  So, this is a comma separated file
        /// and the data contains a comma, we should escape the data with a double quote (someData, "someData,WithComman", other data)</summary>
        char EscapeChar { get; set; }

        /// <summary>Last row number number written (starts at zero indicating nothing written yet).</summary>
        int RowNumber { get; }

        /// <summary>The character that delimits the data.</summary>
        char SplitChar  { get; set; }

        /// <summary>Writes a list of columns to a file.  It will surround any column data with the proper escape character if necessary.</summary>
        void Write(List<string> fieldList);

        /// <summary>Writes a row directly to the file without escaping any data that the columns may contain.</summary>
        void Write(string line);
    }
}
