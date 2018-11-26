using System;
using System.Collections.Generic;
using System.IO;

namespace CsvConverter.RowTools
{
    /// <summary>A class for writing rows to a file</summary>
    public class RowWriter : RowBase, IRowWriter
    {
        private readonly StreamWriter _streamWriter;

        /// <summary>Constructor</summary>
        /// <param name="sw">An instance of the StreamWriter class.</param>
        public RowWriter(StreamWriter sw)
        {
            _streamWriter = sw ?? throw new ArgumentNullException("StreamWriter cannot be null.");
        }

        /// <summary>Used to write out a line of data (Assumes that the split character is already in the line of text</summary>
        /// <param name="line"></param>
        public void Write(string line)
        {
            _streamWriter.WriteLine(line);
            RowNumber++;
        }

        /// <summary>Writes all the data in teh fieldList to a row in the file.</summary>
        /// <param name="fieldList">Data to write to the file.</param>
        public void Write(List<string> fieldList)
        {
            if (fieldList == null)
                throw new ArgumentNullException("fieldList cannot be null.");

            _sb.Length = 0;
            int indexOfLastField = fieldList.Count - 1;
            for (int index = 0; index < fieldList.Count; index++)
            {
                string field = fieldList[index];
                if (field != null)
                {
                    if (field.IndexOf(SplitChar) == -1 && field.IndexOf(EscapeChar) == -1 && field.Contains("\r\n") == false)
                        _sb.Append(field);
                    else
                    {
                        EscapeTheText(field);
                    }
                }

                if (index != indexOfLastField)
                    _sb.Append(SplitChar);
            }

            Write(_sb.ToString());

        }

        private void EscapeTheText(string field)
        {
            _sb.Append(EscapeChar);

            for (int index = 0; index < field.Length; index++)
            {
                char oneChar = field[index];

                // To adhere to RFC 4180, we need to be able to handle text that has a carriage return and line feed with it.
                // If there is both a carriage return and line feed (CRLF) together, we need to write that out to the file.
                if (oneChar == '\r' && index + 1 < field.Length && field[index + 1] == '\n')
                {
                    index++;  // advanced the next since to avoid the line feed \n
                    _sb.AppendLine();
                }
                else
                {
                    if (oneChar == EscapeChar)
                        _sb.Append(EscapeChar);
                    _sb.Append(oneChar);
                }
            }

            _sb.Append(EscapeChar);
        }
    }
}
