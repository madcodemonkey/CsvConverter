using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CsvConverter.Mapper
{
    /// <summary>Used to determine how a CSV column should map on to a property of a class.</summary>
    public class ColumnToPropertyMap 
    {
        private readonly string _defaultColumnName;
        private readonly int _defaultColumnIndex;
        public ColumnToPropertyMap(PropertyInfo propInfo, int columnIndex)
        {
            PropInformation = propInfo;
            ColumnName = propInfo != null ? propInfo.Name : null;
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

        public bool IsDefaultColumnName()
        {
            return string.Compare(ColumnName, _defaultColumnName, true, CultureInfo.InvariantCulture) == 0;
        }

        public bool IsDefaultColumnIndex()
        {
            return _defaultColumnIndex == ColumnIndex;
        }

    }
}