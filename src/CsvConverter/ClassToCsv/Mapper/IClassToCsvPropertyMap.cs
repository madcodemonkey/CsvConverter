using System.Collections.Generic;
using CsvConverter.Mapper;

namespace CsvConverter.ClassToCsv.Mapper
{
    public interface IClassToCsvPropertyMap : IPropertyMap
    {
        bool IgnoreWhenWriting { get; set; }
        string ClassPropertyDataFormat { get; set; }
        IClassToCsvTypeConverter ClassToCsvTypeConverter { get; set; }
        List<IClassToCsvPostConverter> ClassToCsvPostConverters { get; set; }
    }
}