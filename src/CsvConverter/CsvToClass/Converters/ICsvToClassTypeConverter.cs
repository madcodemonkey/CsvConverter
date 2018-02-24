using System;

namespace CsvConverter.CsvToClass
{
    public interface ICsvToClassTypeConverter : ICsvConverter
    {
        /// <summary>This method is called to make sure that the converter can process the string into an object that 
        /// will eventually be assigned to a class property of this type.</summary>
        /// <param name="outputType">The type that should be returned from the Convert method.</param>
        bool CanConvert(Type outputType);

        /// <summary>You are passed the string value and you must convert it to the property type and assign it to the 
        /// class property OR you can use the default converter after doing some manipulation.</summary>
        /// <param name="targetType">The type of the class property the object will be assigned.</param>
        /// <param name="stringValue">The CSV column/field data that we are attempting to convert.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        /// <param name="defaultConverters">The default string to object converter is injected in case you are only making minor tweaks to 
        /// the CSV field input before using the default converter.</param>
        object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverters);
    }
}
