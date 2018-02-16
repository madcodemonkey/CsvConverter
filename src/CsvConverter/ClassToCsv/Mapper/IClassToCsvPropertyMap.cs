using System.Collections.Generic;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{
    public interface IClassToCsvPropertyMap : IPropertyMap
    {
        bool IgnoreWhenWriting { get; set; }
        string ClassPropertyDataFormat { get; set; }
        IClassToCsvTypeConverter ClassPropertyTypeConverter { get; set; }
        List<IClassToCsvPostprocessor> ClassPropertyPostprocessors { get; set; }
    }
}