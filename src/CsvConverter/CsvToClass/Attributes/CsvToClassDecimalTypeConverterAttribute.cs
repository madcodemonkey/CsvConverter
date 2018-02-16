using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public class CsvToClassDecimalTypeConverterAttribute : CsvToClassTypeConverterAttribute, IDecimalConverterSettings
    {
        public CsvToClassDecimalTypeConverterAttribute() : base(typeof(StringToObjectDecimalTypeConverter)) { }
        public CsvToClassDecimalTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }

}
