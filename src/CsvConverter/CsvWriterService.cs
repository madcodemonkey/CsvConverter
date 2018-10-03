﻿using CsvConverter.Mapper;
using CsvConverter.RowTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvConverter
{
    /// <summary>Converts Class into a CSV row.  The class instances will of type T.</summary>
    /// <typeparam name="T">Class instance type</typeparam>
    public class CsvWriterService<T> where T : class, new()
    {
        private const int ColumnIndexDefaultValue = 9999;
        private IRowWriter _rowWriter;
        private List<ColumnToPropertyMap> _columnMapList;
        private bool _headerWritten = false;
        private bool _initialized = false;
       
        /// <summary>Constructor.  This is the standard constructor were you pass in a StreamWriter that is already connected to an open stream.</summary>
        public CsvWriterService(StreamWriter sw) : this(new RowWriter(sw)) { }

        /// <summary>Constructor for dependency injection that is used primarly for TESTING; however, a user could override how 
        /// a row is written by implementing the interface.</summary>
        public CsvWriterService(IRowWriter rowWriter)
        {
            _rowWriter = rowWriter ?? throw new ArgumentNullException(
                "Row writer cannot be null. Note that this constructor is mainly used for testing purposes.");

            DefaultConverterFactory = new DefaultTypeConverterFactory();
        }

        /// <summary>General Configuration settings</summary>
        public CsvConverterConfiguration Configuration { get; private set; } = new CsvConverterConfiguration();

        /// <summary>When generating property maps and a converter is not specified for a known type,
        /// this factory is used to create a converter for the property.</summary>
        public IDefaultTypeConverterFactory DefaultConverterFactory { get; set; }

        /// <summary>Indicates the current row number.</summary>
        public int RowNumber { get { return _rowWriter != null ? _rowWriter.RowNumber : 0; } }

        /// <summary>If called explicitly by the user, it will create the header mappings; otherwise, it will be called
        /// the first time you call WriterRecord.</summary>
        public void Init()
        {
            if (_columnMapList == null)
                _columnMapList = CreateMappings();

            _initialized = true;
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
            foreach (var columnMap in _columnMapList)
            {
                // Ignored columns are NOT written
                 if (columnMap.IgnoreWhenWriting)
                    continue;

                // If record is null, write about a bunch of commas for each column
                if (record == null)
                {
                    row.Add(string.Empty);
                    continue;
                }

                try
                {
                    string value = null;
                    if (columnMap.WriteConverter == null)
                    {
                        throw new CsvConverterException("The column was NOT ignored for writing, but is missing a write converter!");
                    }
                    
                    value = columnMap.WriteConverter.GetWriteData(columnMap.PropInformation.PropertyType,
                                columnMap.PropInformation.GetValue(record), columnMap.ColumnName,
                                columnMap.ColumnIndex, currentRowNumber);
                   
                    foreach (var postConverter in columnMap.PostConverters)
                    {
                        value = postConverter.GetWriteData(columnMap.PropInformation.PropertyType, value, 
                            columnMap.ColumnName, columnMap.ColumnIndex, currentRowNumber);
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

                foreach (var map in _columnMapList)
                {
                    if (map.IgnoreWhenWriting)
                        continue;

                    row.Add(map.ColumnName);
                }

                _rowWriter.Write(row);
            }

            _headerWritten = true;
        }

        private List<ColumnToPropertyMap> CreateMappings()
        {
            var mapper = new ColumnToPropertyMapper<T>(Configuration, DefaultConverterFactory, ColumnIndexDefaultValue);
            return mapper.CreateWriteMap();
        }
    }
}
