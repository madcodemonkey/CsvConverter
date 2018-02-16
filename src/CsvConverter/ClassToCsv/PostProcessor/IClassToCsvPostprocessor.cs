﻿namespace CsvConverter.ClassToCsv
{
    public interface IClassToCsvPostprocessor
    {
        /// <summary>The order that the processor should be processed if there is more than one.</summary>
        int Order { get; set; }
              
        void Initialize(ClassToCsvPostProcessorAttribute attribute);

        /// <summary>Passes in a csv field value (the converter created this string base on the class property).  It can be manipulated in any way you see fit and then 
        /// finally a string is returned for the service to write out to the CSV file.</summary>
        /// <param name="csvField">The csv field.  It may have been manipulated by a previous postprocessor if more than one is specified.</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnIndex">Index of the column</param>
        /// <param name="rowNumber">Row number of the column</param>
        string Work(string csvField, string columnName, int columnIndex, int rowNumber);
    }
}
