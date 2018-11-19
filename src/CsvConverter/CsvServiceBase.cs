using CsvConverter.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvConverter
{
    public abstract class CsvServiceBase
    {
        private IDefaultTypeConverterFactory _defaultConverterFactory = new DefaultTypeConverterFactory();
        protected bool _initialized = false;

        public CsvServiceBase()
        {

        }

        /// <summary>Information about each column. It is not initialized till Init method is called 
        /// and that is done automatically upon using a read or write method.</summary>
        public List<ColumnToPropertyMap> ColumnMapList { get; private set; } = new List<ColumnToPropertyMap>();

        /// <summary>General Configuration settings</summary>
        public CsvConverterConfiguration Configuration { get; private set; } = new CsvConverterConfiguration();

        /// <summary>When generating property maps and a converter is not specified for a known type,
        /// this factory is used to create a converter for the property.</summary>
        public IDefaultTypeConverterFactory DefaultConverterFactory
        {
            get { return _defaultConverterFactory; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("You cannot set the DefaultConverterFactory to null!");
                _defaultConverterFactory = value;
            }
        }

        /// <summary>If called explicitly by the user, it will read the header row and create mappings; otherwise, it will be called
        /// the first time you call a read or write method.</summary>
        public virtual void Init()
        {
            if (ColumnMapList.Count == 0)
            {
                CreateMappings();
            }

            _initialized = true;
        }

        /// <summary>Retrieves a list of Column maps based on the converter being used by the property. 
        /// Call this method AFTER calling Init() or the result will be a count of zero.</summary>
        /// <param name="typeOfConveter">Converter type</param>
        /// <returns>List of columns using the converter.</returns>
        public List<ColumnToPropertyMap> FindColumnsByConverterType(Type typeOfConveter)
        {
            if (_initialized == false)
                return new List<ColumnToPropertyMap>();

            return ColumnMapList
                .Where(w => w.ReadConverter.GetType() == typeOfConveter)
                .ToList();
        }

        /// <summary>Retrieves a list of Column maps based on the class property type.
        /// Call this method AFTER calling Init() or the result will be a count of zero.</summary>
        /// <param name="typeOfProperty">A property type</param>
        /// <returns>List of columns with the specified property type.</returns>
        public List<ColumnToPropertyMap> FindColumnsByPropertyType(Type typeOfProperty)
        {
            if (_initialized == false)
                return new List<ColumnToPropertyMap>();

            return ColumnMapList
                .Where(w => w.PropInformation.PropertyType == typeOfProperty)
                .ToList();
        }

        protected abstract void CreateMappings();


    }
}
