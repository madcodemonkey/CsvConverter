using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public interface ICsvToClassTypeConverter
    {
        /// <summary>This method is called to make sure that the convert can process the string into a object that 
        /// will eventually be assigned to a class property.</summary>
        /// <param name="outputType">The type of class property that it will eventually be used assigned to.</param>
        bool CanOutputThisType(Type outputType);

        /// <summary>You are passed the string value and you must convert it to the property type and assign it to the 
        /// class property OR you can use the default converter after doing some manipulation.</summary>
        /// <param name="targetType">The type of the class property the object will be assigned.</param>
        /// <param name="stringValue">The CSV column/field data that we are attempting to convert.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        /// <param name="defaultConverters">The default string to object converter is injected in case you are only making minor tweaks to 
        /// the CSV field input before using the default converter.</param>
        object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverters);

        /// <summary>Used to pass the attribute to the converter in case it needs any optional inputs.</summary>
        void Initialize(CsvToClassTypeConverterAttribute attribute);
    }
}
