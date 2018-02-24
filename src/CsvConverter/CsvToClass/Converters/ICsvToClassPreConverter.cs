using System;

namespace CsvConverter.CsvToClass
{
    public interface ICsvToClassPreConverter : ICsvConverter
    {
        /// <summary>This method is called to make sure that the pre-converter can handle the csvField string 
        /// when it will eventually be converted into the specified type by a Type Converter.  If it returns false, 
        /// a pre-converter of this type is NOT created for the property and thus is not used.</summary>
        /// <param name="theType">The class property type that it will eventually be used for.</param>
        bool CanConvert(Type theType);

        /// <summary>Passes in a csv field.  It can be manipulated in any way you see fit and then 
        /// finally a string is returned for the service to process.</summary>
        /// <param name="csvField">The csv field.  It may have been manipulated by a previous pre-converter if more than one is specified.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        string Convert(string csvField, string columnName, int columnIndex, int rowNumber);
    }
}
