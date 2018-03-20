using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvConverter.ClassToCsv.Mapper;
using CsvConverter.Mapper;
using CsvConverter.RowTools;

namespace CsvConverter.ClassToCsv
{
    /// <summary>Converts Class int a CSV row.  The class instances will of type T.</summary>
    /// <typeparam name="T">Class instance type</typeparam>
    public class ClassToCsvService<T> where T : class, new()
    {
        private const int ColumnIndexDefaultValue = 9999;
        private IRowWriter _rowWriter;
        private List<IClassToCsvPropertyMap> _csvColumnMapList;
        private bool _headerWritten = false;
        private bool _initialized = false;

        /// <summary>Constructor for dependency injection that is used primarly for testing; however, a user could override how 
        /// a row is written by implementing the interface.</summary>
        public ClassToCsvService(IRowWriter rowWriter)
        {
            _rowWriter = rowWriter ?? throw new ArgumentNullException("Row writer cannot be null. Note that this constructor is mainly used for testing purposes.");
        }

        /// <summary>Constructor.  This is the standard constructor were you pass in a StreamWriter that is already connected to an open stream.</summary>
        public ClassToCsvService(StreamWriter sw)
        {
            _rowWriter = new RowWriter(sw);
        }
        
        /// <summary>General Configuration settings</summary>
        public ClassToCsvConfiguration Configuration { get; private set; } = new ClassToCsvConfiguration();

        /// <summary>Converts the properties on a class to strings that can be written to a CSV field.  It will be injected 
        /// into custom type converters and used when no type converter is specified.</summary>
        public IDefaultObjectToStringTypeConverterManager DefaultConverters { get; set; } = new DefaultObjectToStringTypeConverterManager();

        /// <summary>Indicates the current row number.</summary>
        public int RowNumber { get { return _rowWriter != null ? _rowWriter.RowNumber : 0; } }

        /// <summary>If called explicitly by the user, it will create the header mappings; otherwise, it will be called
        /// the first time you call WriterRecord.</summary>
        public void Init()
        {
            if (_csvColumnMapList == null)
               _csvColumnMapList = CreateMappings();

            _initialized = true;
        }

        /// <summary>Remap columns to match what we read into the system.</summary>
        /// <param name="columnMaps">List of column maps obtained from CsvToClassService</param>
        public void RemapColumns(List<ColumnMap> columnMaps)
        {
            if (_initialized == false)
                Init();

            // Reset all column index
            _csvColumnMapList.Where(w => w.ColumnIndex > 0).ToList().ForEach(item => item.ColumnIndex = ColumnIndexDefaultValue);

            var sortedItems = columnMaps.Where(w => w.ColumnIndex > 0).OrderBy(o => o.ColumnIndex).ToList();
            foreach (var columnMap in sortedItems)
            {
                var propMap = _csvColumnMapList.FirstOrDefault(w => string.Compare(w.ColumnName, columnMap.ColumnName) == 0);
                if (propMap != null)
                {
                    propMap.ColumnIndex = columnMap.ColumnIndex;
                }
                else
                {
                    var newMap = new PropertyMap
                    {
                        ColumnIndex = columnMap.ColumnIndex,
                        ColumnName = columnMap.ColumnName
                    };

                    _csvColumnMapList.Add(newMap);
                }
            }

            _csvColumnMapList = _csvColumnMapList.OrderBy(o => o.ColumnIndex).ToList();
        }

        /// <summary>Writes a single row to the CSV file.</summary>
        /// <param name="record">What to write to the CSV file</param>
        public void WriterRecord(T record)
        {
            if (_initialized == false)
                Init();
            if (_headerWritten == false)
                CreateHeaderRow();              
            
            List<string> dataRow = CreateDataRow(record);
            _rowWriter.Write(dataRow);
        }

        private List<string> CreateDataRow(T record)
        {
            var row = new List<string>();
            int currentRowNumber = _rowWriter.RowNumber + 1;

            // Note: The mapper took out any columns that should NOT be written to the file.
            foreach (var columnMap in _csvColumnMapList)
            {
                // A column that is ignored, but still has a column index IS written to the CSV file.  
                // We just write out a blank (no data).
                if (record == null)
                {
                    row.Add(string.Empty);
                    continue;
                }

                try
                {
                    string value;
                    if (columnMap.ClassToCsvTypeConverter == null)
                    {
                        // Used default converter
                        var someObject = columnMap?.PropInformation?.GetValue(record);
                        if (someObject != null)
                        {
                            value = DefaultConverters.Convert(columnMap.PropInformation.PropertyType,
                                columnMap.PropInformation.GetValue(record), columnMap.ClassPropertyDataFormat,
                                columnMap.ColumnName, columnMap.ColumnIndex, currentRowNumber);
                        }
                        else value = null;
                    }
                    else
                    {
                        // Custom converter found                    
                        value = columnMap.ClassToCsvTypeConverter.Convert(columnMap.PropInformation.PropertyType,
                                columnMap.PropInformation.GetValue(record), columnMap.ClassPropertyDataFormat,
                            columnMap.ColumnName, columnMap.ColumnIndex, currentRowNumber, DefaultConverters);
                    }

                    foreach (var postConverter in columnMap.ClassToCsvPostConverters)
                    {
                        value = postConverter.Convert(value, columnMap.ColumnName, columnMap.ColumnIndex, currentRowNumber);
                    }

                    if (value == null)
                    {
                        row.Add(string.Empty);
                        continue;
                    }

                    row.Add(value.ToString());
                }
                catch (Exception ex)
                {
                    throw new CsvConverterException($"Problem with the {columnMap.ColumnName} column on row {RowNumber}:  {ex.Message}  See the inner exception for more details.", ex);
                }
            }

            return row;
        }

        private void CreateHeaderRow()
        {
            if (Configuration.HasHeaderRow)
            {
                List<string> row = new List<string>();

                // Note: The mapper took out any columns that should NOT be written to the file.
                foreach (var map in _csvColumnMapList)
                {
                    row.Add(map.ColumnName);
                }

                _rowWriter.Write(row);
            }

            _headerWritten = true;
        }

        private List<IClassToCsvPropertyMap> CreateMappings()
        {
            var mapper = new ClassToCsvPropertyMapper<T>();
            return mapper.Map(Configuration, ColumnIndexDefaultValue);
        }
    }
}
