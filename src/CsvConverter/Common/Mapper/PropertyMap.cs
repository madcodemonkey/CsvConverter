using System;
using System.Collections.Generic;
using System.Reflection;
using CsvConverter.ClassToCsv;
using CsvConverter.ClassToCsv.Mapper;
using CsvConverter.CsvToClass;
using CsvConverter.CsvToClass.Mapper;

namespace CsvConverter.Mapper
{

    /// <summary>Used to determine how a CSV column should map on to a property of a class.</summary>
    public class PropertyMap : ICsvToClassPropertyMap, IClassToCsvPropertyMap
    {
        /// <summary>Index of the column in the CSV file.</summary>
        public int ColumnIndex { get; set; }

        /// <summary>Name of the column in the CSV file.</summary>
        public string ColumnName { get; set; }

        /// <summary>Class property information.  This is where the CSV data will go.</summary>
        public PropertyInfo PropInformation { get; set; }
        


        #region Class To CSV
        /// <summary>Indicates that the property should NOT be written into the CSV file.</summary>
        public bool IgnoreWhenWriting { get; set; }

        /// <summary>Used when converting the data into a string.  Any standard C# format is allowed.  It can be used to format numbers, etc.</summary>
        public string ClassPropertyDataFormat { get; set; }

        public List<IClassToCsvPostprocessor> ClassPropertyPostprocessors { get; set; } = new List<IClassToCsvPostprocessor>();


        /// <summary>When writing classes to CSV files, this is an optional converter in case you do NOT want the property 
        /// coverted to a string using the default property type converters</summary>
        public IClassToCsvTypeConverter ClassPropertyTypeConverter { get; set; }
        #endregion // Class To CSV



        #region CSV to Class
        /// <summary>We support one alternate column name</summary>
        public List<string> AltColumnNames { get; set; }

        /// <summary>Indicates that the CSV column should be ignored and its value not not assigned to a class property.</summary>
        public bool IgnoreWhenReading { get; set; }

        /// <summary>When reading CSV files, these are optional preprocesors in case you need to manipulate the string that will eventually be used.</summary>
        public List<ICsvToClassPreprocessor> CsvFieldPreprocessors { get; set; } = new List<ICsvToClassPreprocessor>();

        /// <summary>When reading CSV files, this is an optional converter in case you do NOT want the CSV field coverted to
        /// the same type as the class property or you just want more control over the conversion process.</summary>
        public ICsvToClassTypeConverter CsvFieldTypeConverter { get; set; }
        #endregion // CSV to Class
    }
}
