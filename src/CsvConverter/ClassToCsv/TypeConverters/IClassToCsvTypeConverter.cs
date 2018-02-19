using System;

namespace CsvConverter.ClassToCsv
{
    public interface IClassToCsvTypeConverter
    {
        /// <summary>This method is called to make sure that the converter can process the type into a string.</summary>
        /// <param name="theType">The class property type of the source.</param>
        bool CanHandleThisInputType(Type inputType);

        /// <summary>You are passed the class property information and you must output a string or null that will be written into a CSV file column</summary>
        /// <param name="inputType">The type of the property that is your input.  In most cases it should be a primitive type (int, decimal, etc.).</param>
        /// <param name="value">The object from a class property.  This should be a primitive in most cases.  It's what you get when you call PropertyInfo GetValue method.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="stringFormat">Any string formatting information that was in the main attribute</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        /// <param name="defaultConverters">The default property to string converter is injected in case you are only making minor tweaks to 
        /// the class property but still want the default conversion aferwards.</param>
        /// <returns>A string to write out to the CSV column</returns>
        string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber,
            IObjectToStringDefaultConverters defaultConverters);

        /// <summary>Used to pass the attribute to the converter in case it needs any optional inputs.</summary>
        void Initialize(ClassToCsvTypeConverterAttribute attribute);
    }
}
