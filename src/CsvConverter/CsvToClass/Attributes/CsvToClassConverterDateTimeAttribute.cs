using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>A shortcut way of using the StringToObjectDateTimeTypeConverter with the CsvConverterDateTimeStylesAttribute</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvToClassConverterDateTimeAttribute : CsvConverterDateTimeStylesAttribute
    {
        public CsvToClassConverterDateTimeAttribute() : base(typeof(StringToObjectDateTimeTypeConverter))
        {
        }
    }
}
