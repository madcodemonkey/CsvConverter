using System.Reflection;

namespace CsvConverter.Mapper
{
    /// <summary>Describes a property map's common properties</summary>
    public interface IColumnToPropertyMap
    {
        /// <summary>Index of the column in the CSV file.  This is a one based index.</summary>
        int ColumnIndex { get; set; }

        /// <summary>Name of the column (assuming that the CSV file has a header column)</summary>
        string ColumnName { get; set; }

        /// <summary>Reflection information about the class property</summary>
        PropertyInfo PropInformation { get; set; }
    }
}