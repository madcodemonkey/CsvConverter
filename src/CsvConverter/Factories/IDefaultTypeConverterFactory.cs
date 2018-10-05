using System;

namespace CsvConverter
{
    /// <summary>Creates type converters</summary>
    public interface IDefaultTypeConverterFactory
    {
        /// <summary>Adds a converter to the factory.</summary>
        void AddConverter(Type theType, Type converterType);

        /// <summary>Indicates if a type converter exists in the factory.</summary>
        bool ConverterExists(Type theType);

        /// <summary>Creates the appropriate converter for the class property type</summary>
        /// <param name="theClassPropertyType">The property's type.  This is the class property.</param>
        /// <returns>A type converter</returns>

        ICsvConverter CreateConverter(Type theClassPropertyType);

        /// <summary>Finds a converter in the factory and cast it to the type.</summary>
        T FindConverter<T>(Type theType) where T : class;

        /// <summary>Finds the type of the converter in the factory and returns it.</summary>
        Type FindConverterType(Type theType);

        /// <summary>Removes a type converter from the factory.</summary>
        void RemoveConverter(Type theType);
    }
}
