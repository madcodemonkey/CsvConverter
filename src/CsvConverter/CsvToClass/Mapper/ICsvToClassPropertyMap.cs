using System.Collections.Generic;
using CsvConverter.Mapper;

namespace CsvConverter.CsvToClass.Mapper
{
    public interface ICsvToClassPropertyMap : IPropertyMap
    {
        bool IgnoreWhenReading { get; set; }
        List<string> AltColumnNames { get; set; }
        List<ICsvToClassPreprocessor> CsvFieldPreprocessors { get; set; }
        ICsvToClassTypeConverter CsvFieldTypeConverter { get; set; }
    }
}