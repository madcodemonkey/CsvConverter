using System;
using System.Collections.Generic;
using System.Reflection;

namespace CsvConverter.Mapper
{
    /// <summary>Used to determine how a CSV column should map on to a property of a class.</summary>
    public class ColumnToPropertyMap 
    {
        private readonly string _defaultColumnName;
        private readonly int _defaultColumnIndex;

        /// <summary>Constructor</summary>
        /// <param name="propInfo">PropertyInfo for the class property that the column will map to.</param>
        /// <param name="columnIndex">The index of the column in the CSV file</param>
        public ColumnToPropertyMap(PropertyInfo propInfo, int columnIndex)
        {
            PropInformation = propInfo;
            ColumnName = propInfo?.Name;
            _defaultColumnName = ColumnName;
            ColumnIndex = columnIndex;
            _defaultColumnIndex = columnIndex;
        }

        /// <summary>Index of the column in the CSV file.</summary>
        public int ColumnIndex { get; set; }

        /// <summary>Name of the column in the CSV file.</summary>
        public string ColumnName { get; set; }

        /// <summary>Class property information.  This is where the CSV data will go.</summary>
        public PropertyInfo PropInformation { get; set; }


        #region Writing
        /// <summary>Indicates that the property should NOT be written into the CSV file.</summary>
        public bool IgnoreWhenWriting { get; set; }
  
        /// <summary>When writing CSV files, these are optional postprocesors in case you need to 
        /// manipulate the string that will eventually be written to the csv file.  These are called
        /// after the converters have done their work.</summary>
        public List<ICsvConverterString> PostConverters { get; set; } = new List<ICsvConverterString>();

        /// <summary>When writing classes to CSV files, this is an optional converter in case you do NOT want the property 
        /// coverted to a string using the default property type converters</summary>
        public ICsvConverter WriteConverter { get; set; }
        #endregion 

        #region Reading
        /// <summary>Although you can specify multiple alternate column names ultimately 
        /// only one column can be mapped to a property!</summary>
        public List<string> AltColumnNames { get; set; }

        /// <summary>Indicates that the CSV column should be ignored and its value not not assigned to a class property.</summary>
        public bool IgnoreWhenReading { get; set; }

        /// <summary>When reading CSV files, these are optional preprocesors in case you need to manipulate the string that will eventually be used.</summary>
        public List<ICsvConverterString> PreConverters { get; set; } = new List<ICsvConverterString>();

        /// <summary>When reading CSV files, this is an optional converter in case you do NOT want the CSV field coverted to
        /// the same type as the class property or you just want more control over the conversion process.</summary>
        public ICsvConverter ReadConverter { get; set; }
        #endregion 

        /// <summary>Indicates if the column name has changed from the default name it was given originally.</summary>
        public bool IsDefaultColumnName()
        {
            return string.Compare(ColumnName, _defaultColumnName, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>Indicates if the column index has changed from the default index it was given originally.</summary>
        public bool IsDefaultColumnIndex()
        {
            return _defaultColumnIndex == ColumnIndex;
        }

    }
}