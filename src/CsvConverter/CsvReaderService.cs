using CsvConverter.Mapper;
using CsvConverter.RowTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvConverter
{

    /// <summary>Converts CSV rows into class instances.  The class instances will of type T.</summary>
    /// <typeparam name="T">Class instance type</typeparam>
    public class CsvReaderService<T> where T : class, new()
    {
        private IRowReader _rowReader;
        private Dictionary<int, ColumnToPropertyMap> _columnMapList;
        private int? _columnCount = null;
        private bool _initialized = false;
        private IDefaultTypeConverterFactory _defaultConverterFactory;

        /// <summary>Constructor.  This is the standard constructor were you pass in a StreamReader that is already connected to an
        /// open stream.</summary>
        /// <param name="sr">An instance of StreamReader that is already attached to an open file stream.</param>
        public CsvReaderService(StreamReader sr) : this(new RowReader(sr)) { }

        /// <summary>A constructor for dependency injection that is used primarly for TESTING!; however, a user could override how 
        /// a row is read by implementing the interface.</summary>
        /// <param name="rowReader"></param>
        public CsvReaderService(IRowReader rowReader)
        {
            _rowReader = rowReader ?? throw new ArgumentNullException(
                "Row reader cannot be null. Note that this constructor is mainly used for testing purposes.");

            DefaultConverterFactory = new DefaultTypeConverterFactory();
        }

        /// <summary>General Configuration settings</summary>
        public CsvConverterConfiguration Configuration { get; private set; } = new CsvConverterConfiguration();

        /// <summary>When generating property maps and a converter is not specified for a known type,
        /// this factory is used to create a converter for the property.</summary>
        public IDefaultTypeConverterFactory DefaultConverterFactory
        {
            get { return _defaultConverterFactory;  }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("You cannot set the DefaultConverterFactory to null!");
                _defaultConverterFactory = value;
            }
        }

        /// <summary>If called explicitly by the user, it will read the header row and create mappings; otherwise, it will be called
        /// the first time you call GetRecord.</summary>
        public void Init()
        {
            if (_columnMapList == null)
                _columnMapList = CreateMappings();

            _initialized = true;
        }

        /// <summary>Indicates if there is more to read from the file.</summary>
        /// <returns></returns>
        public bool CanRead()
        {
            return _rowReader != null && _rowReader.CanRead();
        }

        /// <summary>Indicates the current row number.</summary>
        public int RowNumber { get { return _rowReader != null ? _rowReader.RowNumber : 0; } }

        /// <summary>Reads a row, creates an instance of a class of type T and populates the class with data from the row.</summary>
        public T GetRecord()
        {
            if (_initialized == false)
                Init();

            if (CanRead() == false)
                return null;

            List<string> oneRow = _rowReader.ReadRow();

            // Check for blank row.
            if (oneRow == null || (Configuration.IgnoreBlankRows && _rowReader.IsRowBlank))
                return null;

            if (_columnCount.HasValue == false)
                _columnCount = oneRow.Count;
            else if (_columnCount.Value != oneRow.Count && Configuration.ThrowExceptionIfColumnCountChanges)
                throw new ArgumentException($"The column count changed from {_columnCount} to {oneRow.Count} on row number {_rowReader.RowNumber}!");


            var newItem = new T();

            for (int columnIndex = 1; columnIndex <= oneRow.Count; columnIndex++)
            {
                int zeroBasedIndex = columnIndex - 1;
                string fieldValue = oneRow[zeroBasedIndex];
                ColumnToPropertyMap columnMap;
                if (_columnMapList.TryGetValue(columnIndex, out columnMap))
                {
                    if (columnMap.IgnoreWhenReading)
                        continue;

                    try
                    {
                        // Run pre-converters
                        foreach (var preConverter in columnMap.PreConverters)
                        {
                            object result = preConverter.GetReadData(columnMap.PropInformation.PropertyType,
                                fieldValue, columnMap.ColumnName, columnIndex, RowNumber);
                            fieldValue = result == null ? null : (string)result;
                        }

                        // Default OR custom type converter?
                        object newPropertyValue;
                        if (columnMap.ReadConverter == null)
                        {
                            throw new CsvConverterException("The column was NOT ignored for reading, but is missing a read converter!");
                        }

                        newPropertyValue = columnMap.ReadConverter.GetReadData(columnMap.PropInformation.PropertyType,
                            fieldValue, columnMap.ColumnName, columnIndex, RowNumber);

                        columnMap.PropInformation.SetValue(newItem, newPropertyValue);
                    }
                    catch (Exception ex)
                    {
                        throw new CsvConverterException($"Problem with the {columnMap.ColumnName} column on row {RowNumber}:  {ex.Message}  " +
                            $"See the inner exception for more details.", ex);
                    }
                }
                else if (Configuration.IgnoreExtraCsvColumns == false)
                {
                    throw new ArgumentException($"Unable to find a mapping for the CSV column at index {columnIndex} on row number {_rowReader.RowNumber}");
                }
            }

            return newItem;
        }



        const int ColumnIndexDefaultValue = -1;

        /// <summary>Creates the mappings necessary for each property.</summary>
        /// <returns></returns>
        private Dictionary<int, ColumnToPropertyMap> CreateMappings()
        {
            // Retrieve the header row if the file has one!
            List<string> headerColumns = Configuration.HasHeaderRow ? _rowReader.ReadRow() : new List<string>();

            // Map the class properties to a mapper
            var mapper = new ColumnToPropertyMapper<T>(Configuration, DefaultConverterFactory, ColumnIndexDefaultValue);
            return mapper.CreateReadMap(headerColumns);
        }

    }
}
