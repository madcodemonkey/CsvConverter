using System;

namespace CsvConverter
{
    /// <summary>Main interface for all converters</summary>
    public interface ICsvConverter
    {
        /// <summary>Used to pass the attribute to the converter in case it needs any optional inputs.</summary>
        void Initialize(CsvConverterBaseAttribute attribute, IDefaultTypeConverterFactory defaultFactory);

        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        bool CanRead(Type propertyType);


        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        bool CanWrite(Type propertyType);

        /// <summary>You are passed the class property information and you must output a string or null that will be written into a CSV file column</summary>
        /// <param name="inputType">The type of the property that is your input.  In most cases it should be a primitive type (int, decimal, etc.).</param>
        /// <param name="value">The object from a class property.  This should be a primitive in most cases.  It's what you get when you call PropertyInfo GetValue method.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        /// <returns>A string to write out to the CSV column</returns>
        string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber);


        /// <summary>You are passed the string value and you must convert it to the property type and assign it to the 
        /// class property OR you can use the default converter after doing some manipulation.</summary>
        /// <param name="inputType">The type of the class property the object will be assigned.</param>
        /// <param name="value">The CSV column/field data that we are attempting to convert.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        /// <returns>An object to assign to a class property</returns>
        object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber);
    }
}
