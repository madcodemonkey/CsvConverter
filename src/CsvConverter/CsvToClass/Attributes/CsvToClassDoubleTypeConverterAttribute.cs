using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public class CsvToClassDoubleTypeConverterAttribute : CsvToClassTypeConverterAttribute, IDoubleConverterSettings
    {
        public CsvToClassDoubleTypeConverterAttribute() : base(typeof(StringToObjectDoubleTypeConverter)) { }
        public CsvToClassDoubleTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }

}
