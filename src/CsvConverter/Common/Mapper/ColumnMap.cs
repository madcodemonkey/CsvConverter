using System.Collections.Generic;

namespace CsvConverter.Mapper
{
    /// <summary>Used for public consumption when information about internal mappings is requried</summary>
    public class ColumnMap
    {
        /// <summary>Indicates that the CSV column should be ignored.</summary>
        public bool IgnoreWhenReading { get; set; }

        /// <summary>Index of the column in the CSV file.</summary>
        public int ColumnIndex { get; set; }

        /// <summary>Name of the column in the CSV file.</summary>
        public string ColumnName { get; set; }

        /// <summary>We support one alternate column name</summary>
        public List<string> AltColumnNames { get; set; }
    }
}
