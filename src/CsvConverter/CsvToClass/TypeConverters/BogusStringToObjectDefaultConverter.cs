using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    /// <summary>This is passed to the default converters to remind anybody that has overriden a default
    /// converter that this is no fallback/default converter to they can use.  Default converters are passed
    /// to custom converters!!  Default converters must standalone!!</summary>
    internal class BogusStringToObjectDefaultConverter : IStringToObjectDefaultConverters
    {
        public void AddConverter(Type outputType, ICsvToClassTypeConverter converter)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public bool ConverterExists(Type typeToConvert)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public ICsvToClassTypeConverter FindConverter(Type theType)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public T FindConverter<T>(Type theType) where T : class
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void RemoveConverter(Type typeToConvert)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDateTimeParsing(IDateConverterSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDecimalSettings(IDecimalPlacesSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDoubleSettings(IDecimalPlacesSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }
    }
}
