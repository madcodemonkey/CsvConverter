using System;
using CsvConverter.TypeConverters;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterDecimalPlacesAttribute : CsvConverterCustomAttribute, IDoubleConverterSettings
    {
        public CsvConverterDecimalPlacesAttribute(Type typeConverter) : base(typeConverter) { }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }
}
