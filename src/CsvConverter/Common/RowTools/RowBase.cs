using System.Text;

namespace CsvConverter.RowTools
{
    public abstract class RowBase
    {
        protected const char EscapeChar = '"';
        protected const char SplitChar = ',';
        protected StringBuilder _sb = new StringBuilder();

        public int RowNumber { get; protected set; }

    }
}
