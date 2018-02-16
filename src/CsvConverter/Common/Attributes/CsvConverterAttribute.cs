using System;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvConverterAttribute : Attribute
    {
        /// <summary>READING CSV FILES: It is used ONLY when there is NOT a header row.  If the CSV file has a header row this is ignored.  
        /// If there is NOT a header row, this is mandatory and represents the column index (zero based) position of the column in the CSV file.
        /// WRITING CSV FILES: It is used to determine the column order of the CSV files.  If not specified, we will order by ColumnName.</summary>
        public int ColumnIndex { get; set; } = int.MaxValue;

        /// <summary>READING CSV FILES:  Used ONLY when there IS a header row.  Use this if the CSV column name does NOT match the property name 
        /// (case doesn't matter when matching).  Only one column can be mapped to a property!  This is the primary column name and will
        /// be used BEFORE AltColumnNames!
        /// WRITING CSV FILES: If specified, this will be the header column name.  If not specified, the property name will be used.</summary>
        public string ColumnName { get; set; }

        /// <summary>READING CSV FILES: Used ONLY when there IS a header row and ColumnName was not used.  Use commas to specify more than one column name.  
        /// Only one column can be mapped to a property!
        /// WRITING CSV FILES:  Not used!</summary>
        public string AltColumnNames { get; set; }

        /// <summary>READING CSV FILES: Not used!
        /// WRITING CSV FILES:  Used when converting the data into a string.  Any standard C# format is allowed.  It can be used to format numbers, etc.</summary>
        public string DataFormat { get; set; }

        /// <summary>READING CSV FILES:  Ignore this property regardless if it is used in the CSV or if it has nothing to do with the CSV file.
        /// WRITING CSV FILES:  Not used!</summary>
        public bool IgnoreWhenReading { get; set; } = false;
        
        /// <summary>WRITING CSV FILES: Prevents data from being written to the CSV file. If ColumnIndex == -1 AND this is set to true, it is used to 
        /// prevent column from being created in the CSV file.
        /// READING CSV FILES:  Not used.</summary>
        public bool IgnoreWhenWriting { get; set; } = false;

    }
}
