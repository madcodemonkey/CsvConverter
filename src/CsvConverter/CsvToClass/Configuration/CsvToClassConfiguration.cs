using System;

namespace CsvConverter.CsvToClass
{
    public class CsvToClassConfiguration
    {
        /// <summary>Indicates if the CSV file has a header row.  If so, it will be read and matched to class properties</summary>
        public bool HasHeaderRow { get; set; } = true;

        /// <summary>While mapping CSV columns to class properties, this indicates if an error should be thrown if a CSV column cannot 
        /// be matched to a class property.  By default this is false assuming that if the user was interested in the CSV column they 
        /// would create a property for it. However, the reverse is NOT true in that if an existing class property that is NOT marked 
        /// with ignore cannot be matched to a CSV column you will always get an error.  The class property should be marked with the 
        /// CsvConverterAttribute and set IgnoreWhenReading to true if it truly should be ignored.</summary>
        public bool IgnoreExtraCsvColumns { get; set; } = true;

        /// <summary>Indicates if the column count changes from row to row we should throw an exception.  The column count should be consistent throughout the entire file.
        /// It really should NOT change from row to row or it is truly malformed.</summary>
        public bool ThrowExceptionIfColumnCountChanges { get; set; } = true;

        /// <summary>Indicates that if a blank row (all commas) is encountered, that it should be ignored.</summary>
        public bool IgnoreBlankRows { get; set; } = true;
    }
}
