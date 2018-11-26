using CsvConverter.Mapper;
using System;
using System.Collections.Generic;

namespace CsvConverter
{
    public interface ICsvReaderService<T> where T : class, new()
    {
        /// <summary>Indicates the current row number.</summary>
        int RowNumber { get; }

        /// <summary>Indicates if there is more to read from the file.</summary>
        bool CanRead();

        /// <summary>Reads a row, creates an instance of a class of type T and populates the class with data from the row.</summary>
        T GetRecord();

        /// <summary>Information about each column. It is not initialized till Init method is called 
        /// and that is done automatically upon using a read or write method.</summary>
        List<ColumnToPropertyMap> ColumnMapList { get; }

        /// <summary>General Configuration settings</summary>
        CsvConverterConfiguration Configuration { get; }

        /// <summary>When generating property maps and a converter is not specified for a known type,
        /// this factory is used to create a converter for the property.</summary>
        IDefaultTypeConverterFactory DefaultConverterFactory { get; set; }

        /// <summary>Retrieves a list of Column maps based on the converter being used by the property. 
        /// Call this method AFTER calling Init() or the result will be a count of zero.</summary>
        /// <param name="typeOfConveter">Converter type</param>
        /// <returns>List of columns using the converter.</returns>
        List<ColumnToPropertyMap> FindColumnsByConverterType(Type typeOfConveter);

        /// <summary>Retrieves a list of Column maps based on the class property type.
        /// Call this method AFTER calling Init() or the result will be a count of zero.</summary>
        /// <param name="typeOfProperty">A property type</param>
        /// <returns>List of columns with the specified property type.</returns>
        List<ColumnToPropertyMap> FindColumnsByPropertyType(Type typeOfProperty);

        /// <summary>If called explicitly by the user, it will read the header row and create mappings; otherwise, it will be called
        /// the first time you call a read or write method.</summary>
        void Init();


    }
}