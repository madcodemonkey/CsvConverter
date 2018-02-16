using System;
using System.Collections.Generic;
using System.IO;

namespace CsvConverter.RowTools
{
    public class RowReader : RowBase, IRowReader
    {
        private StreamReader _streamReader;
        private int _lengthBeforeExit;
        public RowReader(StreamReader sr)
        {
            _streamReader = sr ?? throw new ArgumentNullException("StreadReader cannot be null.");
        }
        public bool IsRowBlank { get; private set; } = true;
        public int LastColumnCount { get; private set; }

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
                switch (c)
                {
                    case EscapeChar:
                        if (!inEscape)
                            inEscape = true;
                        else
                        {
                            if (!priorEscape)
                            {
                                if (i + 1 < oneLine.Length && oneLine[i + 1] == EscapeChar)
                                    priorEscape = true;
                                else
                                    inEscape = false;
                            }
                            else
                            {
                                _sb.Append(c);
                                priorEscape = false;
                            }
                        }
                        break;
                    case SplitChar:
                        if (inEscape) //if in escape
                            _sb.Append(c);
                        else
                        {
                            AddColumn(result, _sb.ToString());
                            _sb.Length = 0;
                        }
                        break;
                    default:
                        _sb.Append(c);
                        break;
                }

                if (inEscape && i == _lengthBeforeExit)
                {
                    // According to RFC 4180 (formatting CSV files)- https://www.ietf.org/rfc/rfc4180.txt
                    // You should be able to read over multiple rows if a carrage return is encounted within 
                    // quotes.
                    _sb.Append("\r\n");
                    oneLine = ReadOneLine();
                    if (oneLine == null)
                        break;  // should only hit this in testing!
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
