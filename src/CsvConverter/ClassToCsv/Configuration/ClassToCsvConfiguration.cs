using System;

namespace CsvConverter.ClassToCsv
{
    public class ClassToCsvConfiguration
    {
        /// <summary>Indicates if we should write a header row in the CSV file.</summary>
        public bool HasHeaderRow { get; set; } = true;
    }
}
