using System;
using System.Collections.Generic;
using System.IO;

namespace CsvConverter.RowTools
{
    /// <summary>A class for reading rows.</summary>
    public class RowReader : RowBase, IRowReader
    {
        private readonly StreamReader _streamReader;
        private int _lengthBeforeExit;

        /// <summary>Constructor</summary>
        /// <param name="sr">An instance of a StreamReader class.</param>
        public RowReader(StreamReader sr)
        {
            _streamReader = sr ?? throw new ArgumentNullException(nameof(sr), "StreamReader cannot be null.");
        }

        /// <summary>Indicates if a row is blank (contains no data)</summary>
        public bool IsRowBlank { get; private set; } = true;

        /// <summary>The column count of the previous row.  Column count should remain the same across all rows.</summary>
        public int LastColumnCount { get; private set; }

        /// <summary>Indicates if we can read more data.</summary>
        /// <returns></returns>
        public bool CanRead()
        {
            return _streamReader.EndOfStream == false;
        }

        /// <summary>Read one row from a stream.</summary>
        /// <returns>List of strings</returns>
        /// <remarks>This code is based on code by Alex Begun with modifications from me to adhere to RFC 4180.
        /// https://stackoverflow.com/a/41250748/97803</remarks>
        public List<string> ReadRow()
        {
            IsRowBlank = true;
            _sb.Length = 0;
            var result = LastColumnCount > 0 ? new List<string>(LastColumnCount) : new List<string>();

            if (CanRead() == false)
                return result;

            string oneLine = ReadOneLine();

            bool inEscape = false;
            bool priorEscape = false;
            for (int i = 0; i < oneLine.Length; i++)
            {
                char c = oneLine[i];
                if (c == EscapeChar)
                {
                    if (!inEscape)
                    {
                        inEscape = true;
                    }
                    else
                    {
                        // Trying to escape an escape character?  
                        // Quotes are usually the escape character and they are escaped
                        // by putting two next to each other.
                        if (!priorEscape)
                        {
                            if (i + 1 < oneLine.Length && oneLine[i + 1] == EscapeChar)
                            {
                                priorEscape = true;
                            }
                            else
                            {
                                inEscape = false;
                            }
                        }
                        else
                        {
                            _sb.Append(c);
                            priorEscape = false;
                        }
                    }
                }
                else if (c == SplitChar)
                {
                    if (inEscape)
                    {
                        _sb.Append(c);
                    }
                    else
                    {
                        AddColumn(result, _sb.ToString());
                        _sb.Length = 0;
                    }
                }
                else
                {
                    _sb.Append(c);
                }

                if (inEscape && i == _lengthBeforeExit)
                {
                    // According to RFC 4180 (formatting CSV files)- https://www.ietf.org/rfc/rfc4180.txt
                    // You should be able to read over multiple rows if a carriage return is encountered within 
                    // quotes.

                    // We've had a special case where the quoted data contains a couple of carriage returns
                    // thus resulting in lines with null or empty strings in them.  Keep trying to get to a
                    // line with data in it.  Even a space is ok.  We need to get to that next escape character.
                    do
                    {
                        _sb.Append("\r\n");
                        if (CanRead() == false)
                            break;
                        oneLine = ReadOneLine();
                    }
                    while (string.IsNullOrEmpty(oneLine)); // Space is OK!
                   
                    i = -1; // For loop above will increment it to zero
                }
            }

            AddColumn(result, _sb.ToString());

            LastColumnCount = result.Count;

            return result;
        }

        private string ReadOneLine()
        {
            string oneLine = _streamReader.ReadLine();
            if (oneLine != null)
                _lengthBeforeExit = oneLine.Length - 1;
            else _lengthBeforeExit = 0;
            RowNumber++;
            return oneLine;
        }

        private void AddColumn(List<string> columns, string data)
        {
            if (IsRowBlank && string.IsNullOrWhiteSpace(data) == false)
                IsRowBlank = false;
            columns.Add(data);
        }
    }
}
