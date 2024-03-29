﻿using CsvConverter.Mapper;
using CsvConverter.RowTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvConverter
{
    /// <summary>Converts Class into a CSV row.  The class instances will of type T.
    ///<see href="https://github.com/madcodemonkey/CsvConverter/wiki/Writing">Documentation here</see> </summary>
    /// <typeparam name="T">Class instance type</typeparam>
    public class CsvWriterService<T> : CsvServiceBase, ICsvWriterService<T> where T : class, new()
    {
        private const int ColumnIndexDefaultValue = 9999;
        private readonly IRowWriter _rowWriter;
        private bool _headerWritten = false;
       
        /// <summary>Constructor.  This is the standard constructor were you pass in a StreamWriter that is already connected to an open stream.</summary>
        public CsvWriterService(StreamWriter sw) : this(new RowWriter(sw)) { }

        /// <summary>Constructor for dependency injection that is used primarily for TESTING; however, a user could override how 
        /// a row is written by implementing the interface.</summary>
        public CsvWriterService(IRowWriter rowWriter)
        {
            _rowWriter = rowWriter ?? throw new ArgumentNullException(nameof(rowWriter),
                "Row writer cannot be null. Note that this constructor is mainly used for testing purposes.");
        }
   
        /// <summary>Indicates the current row number.</summary>
        public int RowNumber { get { return _rowWriter != null ? _rowWriter.RowNumber : 0; } }

        /// <summary>If called explicitly by the user, it will read the header row and create mappings; otherwise, it will be called
        /// the first time you call a read or write method.</summary>
        public override void Init()
        {
            _rowWriter.EscapeChar = this.Configuration.EscapeChar;
            _rowWriter.SplitChar = this.Configuration.SplitChar;
            base.Init();
        }

        /// <summary>Writes a single row to the CSV file.</summary>
        /// <param name="record">What to write to the CSV file</param>
        public void WriteRecord(T record)
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
            foreach (var columnMap in ColumnMapList)
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

                foreach (var map in ColumnMapList)
                {
                    if (map.IgnoreWhenWriting)
                        continue;

                    row.Add(map.ColumnName);
                }

                _rowWriter.Write(row);
            }

            _headerWritten = true;
        }

        /// <summary>Creates the column to property mapper</summary>
        protected override void CreateMappings()
        {
            ColumnMapList.Clear();
            var mapper = new ColumnToPropertyMapper<T>(Configuration, DefaultConverterFactory, ColumnIndexDefaultValue);
            ColumnMapList.AddRange(mapper.CreateWriteMap());
        }

    }
}
