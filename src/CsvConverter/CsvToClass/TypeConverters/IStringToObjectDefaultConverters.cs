using System;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    public interface IStringToObjectDefaultConverters
    {
        /// <summary>Adds a converter for a given type.</summary>
        /// <param name="outputType">The output Type of the converter</param>
        /// <param name="converter">Method to call when type is encountered</param>
        void AddConverter(Type outputType, ICsvToClassTypeConverter converter);

        /// <summary>Converts the string value into an object that will eventually be assigned to a class property.</summary>
        /// <param name="stringValue">String to convert</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber);

        /// <summary>Finds the converter for the specified type.</summary>
        /// <param name="theType">The type to convert</param>
        ICsvToClassTypeConverter FindConverter(Type theType);

        /// <summary>Finds a converter and attempts to cast it to the type specified.</summary>
        /// <typeparam name="T">Type to cast the conveter to</typeparam>
        /// <param name="theType">The type that the conveter converts</param>
        T FindConverter<T>(Type theType) where T : class;

        /// <summary>Indicates if a converter exists.  This is usually used prior to adding a converter to avoid an exception.</summary>
        /// <param name="typeToConvert">Type to convert</param>
        bool ConverterExists(Type typeToConvert);

        /// <summary>Removes a converter for a given type.</summary>
        /// <param name="typeToConvert">Type of converter to remove.</param>
        void RemoveConverter(Type typeToConvert);

        /// <summary>Updates the date settings of the double converter if it implements the IDoubleConverterSettings interface.</summary>
        void UpdateDoubleSettings(IDoubleConverterSettings settings);

        /// <summary>Updates the date settings of the decimal converter if it implements the IDecimalConverterSettings interface.</summary>
        void UpdateDecimalSettings(IDecimalConverterSettings settings);

        /// <summary>Updates the date settings of the datetime converter if it implements the IDateConverterSettings interface.  Using these
        /// settings will case the DateTime.ParseExact method to be used as opposed to DateTime.Parse.</summary>
        void UpdateDateTimeParsing(IDateConverterSettings settings);
    }
}