using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.ClassToCsv
{
    /// <summary>This is passed to the default converters to remind anybody that has overriden a default
    /// converter that this is no fallback/default converter to they can use.  Default converters are passed
    /// to custom converters!!  Default converters must standalone!!</summary>
    internal class BogusObjectToStringDefaultConverters : IObjectToStringDefaultConverters
    {
        public void AddConverter(Type theType, IClassToCsvTypeConverter converter)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public string Convert(Type theType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public bool ConverterExists(Type theType)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public T FindConverter<T>(Type theType) where T : class
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void RemoveConverter(Type theType)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateBooleanSettings(IBooleanConverterSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }
    }
}
