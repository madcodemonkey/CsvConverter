using System;
using System.Collections.Generic;
using System.IO;
using CsvConverter.CsvToClass.Mapper;
using CsvConverter.Mapper;
using CsvConverter.RowTools;

namespace CsvConverter.CsvToClass
{
    public class CsvToClassService<T> where T : class, new()
    {
        private IRowReader _rowReader;
        private Dictionary<int, ICsvToClassPropertyMap> _csvColumnMapList;
        private int? _columnCount = null;
        private bool _initialized = false;

        public CsvToClassService(IRowReader rowReader)
        {
            _rowReader = rowReader ?? throw new ArgumentNullException("Row reader cannot be null. Note that this constructor is mainly used for testing purposes.");
        }

        public CsvToClassService(StreamReader sr)
        {
            _rowReader = new RowReader(sr);
        }

        /// <summary>General Configuration settings</summary>
        public CsvToClassConfiguration Configuration { get; private set; } = new CsvToClassConfiguration();

        /// <summary>The string to object converter.  Used for converting strings into property values.  It will be injected 
        /// into custom type converters and used when no type converter is specified.</summary>
        public IDefaultStringToObjectTypeConverterManager DefaultConverters { get; set; } = new DefaultStringToObjectTypeConverterManager();

        public void Init()
        {
            if (_csvColumnMapList == null)
                _csvColumnMapList = CreateMappings();

            _initialized = true;
        }

        public bool CanRead()
        {
            return _rowReader != null && _rowReader.CanRead();
        }

        public int RowNumber { get { return _rowReader != null ? _rowReader.RowNumber : 0; } }

        public T GetRecord()
        {
            if (_initialized == false)
                Init();

            if (CanRead() == false)
                return null;

            List<string> oneRow = _rowReader.ReadRow();

            // Check for blank row.
            if (Configuration.IgnoreBlankRows && _rowReader.IsRowBlank)
                return null;

            if (_columnCount.HasValue == false)
                _columnCount = oneRow.Count;
            else if (_columnCount.Value != oneRow.Count && Configuration.ThrowExceptionIfColumnCountChanges)
                throw new ArgumentException($"The column count changed from {_columnCount} to {oneRow.Count} on row number {_rowReader.RowNumber}!");


            var newItem = new T();

            for (int columnIndex = 0; columnIndex < oneRow.Count; columnIndex++)
            {
                string fieldValue = oneRow[columnIndex];
                ICsvToClassPropertyMap mapping;
                if (_csvColumnMapList.TryGetValue(columnIndex, out mapping))
                {
                    if (mapping.IgnoreWhenReading)
                        continue;

                    // Run pre-converters
                    foreach (var preConverter in mapping.CsvToClassPreConverters)
                    {
                        fieldValue = preConverter.Work(fieldValue, mapping.ColumnName, columnIndex, RowNumber);
                    }

                    // Default OR custom type converter?
                    object newPropertyValue = mapping.CsvToClassTypeConverter == null ?
                        // DEFAULT
                        DefaultConverters.Convert(mapping.PropInformation.PropertyType, fieldValue, mapping.ColumnName, columnIndex, RowNumber) :
                        // CUSTOM
                        mapping.CsvToClassTypeConverter.Convert(mapping.PropInformation.PropertyType, fieldValue,
                            mapping.ColumnName, columnIndex, RowNumber, DefaultConverters);

                    mapping.PropInformation.SetValue(newItem, newPropertyValue);
                }
                else if (Configuration.IgnoreExtraCsvColumns == false)
                {
                    throw new ArgumentException($"Unable to find a mapping for the CSV column at index {columnIndex} on row number {_rowReader.RowNumber}");
                }
            }

            return newItem;
        }

        public List<ColumnMap> GetColumnMaps()
        {
            var result = new List<ColumnMap>();

            if (_csvColumnMapList != null)
            {
                foreach (var map in _csvColumnMapList.Values)
                {
                    var oneMapCopy = new ColumnMap()
                    {
                        ColumnName = map.ColumnName,
                        ColumnIndex = map.ColumnIndex,
                        IgnoreWhenReading = map.IgnoreWhenReading
                    };

                    if (map.AltColumnNames == null || map.AltColumnNames.Count == 0)
                        oneMapCopy.AltColumnNames = new List<string>();
                    else oneMapCopy.AltColumnNames = new List<string>(map.AltColumnNames);

                    result.Add(oneMapCopy);
                }
            }

            return result;
        }

        private Dictionary<int, ICsvToClassPropertyMap> CreateMappings()
        {
            List<string> headerColumns = Configuration.HasHeaderRow ? _rowReader.ReadRow() : new List<string>();

            var mapper = new CsvToClassPropertyMapper<T>();
            return mapper.Map(headerColumns, Configuration);
        }
    }
}
