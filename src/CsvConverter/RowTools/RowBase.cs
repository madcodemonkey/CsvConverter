using System.Text;

namespace CsvConverter.RowTools
{
    /// <summary>Abstract base class that describes a row in a CSV file.</summary>
    public abstract class RowBase
    {
        /// <summary>The Escape character needed if the SplitChar is found in the actual value in a column.  So, this is a comma separated file
        /// and the data contains a comma, we should escape the data with a double quote (someData, "someData,WithComman", other data)</summary>
        protected const char EscapeChar = '"';

        /// <summary>The character that delimits the data.</summary>
        protected const char SplitChar = ',';

        /// <summary>A string builder that can be used for row data.</summary>
        protected StringBuilder _sb = new StringBuilder();

        /// <summary>The row number in the file.</summary>
        public int RowNumber { get; protected set; }
    }
}
