using System.Collections.Generic;

namespace CsvConverter.RowTools
{
    /// <summary>Used for writing rows to a stream.</summary>
    public interface IRowWriter
    {
        /// <summary>Last row number number written (starts at zero indicating nothing written yet).</summary>
        int RowNumber { get; }

        /// <summary>Writes a list of columns to a file.  It will surround any column data with the proper escape character if necessary.</summary>
        void Write(List<string> fieldList);

        /// <summary>Writes a row directly to the file without escaping any data that the columns may contain.</summary>
        void Write(string line);
    }
}