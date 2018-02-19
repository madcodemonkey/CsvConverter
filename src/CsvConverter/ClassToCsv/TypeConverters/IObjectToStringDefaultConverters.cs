using CsvConverter.TypeConverters;
using System;

namespace CsvConverter.ClassToCsv
{
    public interface IObjectToStringDefaultConverters
    { 
        string Convert(Type theType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber);

        /// <summary>Adds a converter for a given type.</summary>
        /// <param name="theType">Type to convert</param>
        /// <param name="converter">A converter</param>
        void AddConverter(Type theType, IClassToCsvTypeConverter converter);

        /// <summary>Indicates if a converter exists.  This is usually used prior to adding a converter to avoid an exception.</summary>
        /// <param name="theType">Type to convert</param>
        bool ConverterExists(Type theType);

        /// <summary>Finds a converter and attempts to cast it to the type specified.</summary>
        /// <typeparam name="T">Type to cast the conveter to</typeparam>
        /// <param name="theType">The type that the converter converts</param>
        T FindConverter<T>(Type theType) where T : class;

        /// <summary>Removes a converter for a given type.</summary>
        /// <param name="theType">Type of converter to remove.</param>
        void RemoveConverter(Type theType);

        /// <summary>Updates the settings of the boolean converter if it implements the IBooleanConverterSettings interface.</summary>
        void UpdateBooleanSettings(IBooleanConverterSettings settings);
    }
}
