﻿using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>A shortcut way of using the StringToObjectDoubleTypeConverter with the CsvConverterDecimalPlacesAttribute</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvToClassConverterDoubleAttribute : CsvConverterDecimalPlacesAttribute
    {
        public CsvToClassConverterDoubleAttribute() : base(typeof(StringToObjectDoubleTypeConverter))
        {
        }
    }
}
