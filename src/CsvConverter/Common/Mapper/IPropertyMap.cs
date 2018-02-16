using System.Reflection;

namespace CsvConverter.Mapper
{
    public interface IPropertyMap
    {
        int ColumnIndex { get; set; }
        string ColumnName { get; set; }
        PropertyInfo PropInformation { get; set; }
    }
}