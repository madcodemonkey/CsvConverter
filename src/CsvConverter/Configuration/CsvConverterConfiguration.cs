namespace CsvConverter
{
    /// <summary>Main configuration class.</summary>
    public class CsvConverterConfiguration
    {
        /// <summary>Reading.  Indicates that you would like null returned if the line is blank (all white space, null or all commas) 
        /// rather than letting the reader try to interpret the blank as data and throw an exception.</summary>
        public bool BlankRowsAreReturnedAsNull { get; set; } = true;

        /// <summary>The Escape character needed if the SplitChar is found in the actual value in a column.  So, this is a comma separated file
        /// and the data contains a comma, we should escape the data with a double quote (someData, "someData,WithComman", other data)</summary>
        public char EscapeChar { get; set; } = '"';

        /// <summary>Reading or Writing.  Indicates if a CSV file has a header row.</summary>
        public bool HasHeaderRow { get; set; } = true;

        /// <summary>Reading. While mapping CSV columns to class properties, this indicates if an error should be thrown if a CSV column cannot 
        /// be matched to a class property.  By default this is false assuming that if the user was interested in the CSV column they 
        /// would create a property for it. However, the reverse is NOT true in that if an existing class property that is NOT marked 
        /// with ignore cannot be matched to a CSV column you will always get an error.  The class property should be marked with the 
        /// CsvConverterAttribute and set IgnoreWhenReading to true if it truly should be ignored.</summary>
        public bool IgnoreExtraCsvColumns { get; set; } = true;

        /// <summary>The character that delimits the data.</summary>
        public char SplitChar  { get; set; }  = ',';

        /// <summary>Reading.Indicates if the column count changes from row to row we should throw an exception.  The column count should be consistent throughout the entire file.
        /// It really should NOT change from row to row or it is truly malformed.</summary>
        public bool ThrowExceptionIfColumnCountChanges { get; set; } = true;
    }
}
