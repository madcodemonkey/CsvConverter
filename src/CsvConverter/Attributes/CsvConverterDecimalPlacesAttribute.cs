﻿using System;
using CsvConverter.TypeConverters;

namespace CsvConverter
{
    /// <summary>Used for specifying the number of decimal places that a double or decimal should have.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterDecimalPlacesAttribute : CsvConverterCustomAttribute, IDecimalPlacesSettings
    {
        public CsvConverterDecimalPlacesAttribute(Type typeConverter) : base(typeConverter) { }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }
}