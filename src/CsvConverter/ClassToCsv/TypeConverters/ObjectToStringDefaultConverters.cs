using System;
using System.Collections.Generic;
using CsvConverter.TypeConverters;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringDefaultConverters : IObjectToStringDefaultConverters
    {
        public ObjectToStringDefaultConverters()
        {
            RegisterBuiltInConverters();
        }
         
        /// <summary>Converts a property into a string with an optional string format.</summary>
        /// <param name="theType">The type to convert.  Currently it only handles primitives.</param>
        /// <param name="value">The value in the property represented as an object.  This is what you get when you call PropertyInfo object GetValue method.</param>
        /// <param name="stringFormat">Any special formatting</param>
        /// <returns></returns>
        public string Convert(Type theType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber)
        {
            if (value == null)
               return null;

            if (_converters.ContainsKey(theType))
            {

                return _converters[theType].Convert(theType, value, stringFormat, columnName, columnIndex, rowNumber, _bogusConverter);
            }

            return value.ToString();
        }

        public void AddConverter(Type theType, IClassToCsvTypeConverter callback)
        {
            _converters.Add(theType, callback);
        }

        public T FindConverter<T>(Type theType) where T : class
        {
            if (ConverterExists(theType) == false)
                return null;

            return _converters[theType] as T;
        }

        public void RemoveConverter(Type theType)
        {
            _converters.Remove(theType);
        }

        public bool ConverterExists(Type theType)
        {
            return _converters.ContainsKey(theType);
        }

        public void UpdateBooleanSettings(IBooleanConverterSettings settings)
        {
            var converter = FindConverter<IBooleanConverterSettings>(typeof(bool));
            if (converter != null)
                converter.OutputFormat = settings.OutputFormat;

        }

        private Dictionary<Type, IClassToCsvTypeConverter> _converters = new Dictionary<Type, IClassToCsvTypeConverter>();
        private IObjectToStringDefaultConverters _bogusConverter = new BogusObjectToStringDefaultConverters();


        private void RegisterBuiltInConverters()
        {
            var intConverter = new ObjectToStringIntTypeConverter();
            AddConverter(typeof(int), intConverter);
            AddConverter(typeof(int?), intConverter);

            var decimalConverter = new ObjectToStringDecimalTypeConverter();
            AddConverter(typeof(decimal), decimalConverter);
            AddConverter(typeof(decimal?), decimalConverter);

            var doubleConverter = new ObjectToStringDoubleTypeConverter();
            AddConverter(typeof(double), doubleConverter);
            AddConverter(typeof(double?), doubleConverter);

            var byteConverter = new ObjectToStringByteTypeConverter();
            AddConverter(typeof(byte), byteConverter);
            AddConverter(typeof(byte?), byteConverter);

            var booleanConverter = new ObjectToStringBooleanTypeConverter();
            AddConverter(typeof(bool), booleanConverter);
            AddConverter(typeof(bool?), booleanConverter);

            var shortConverter = new ObjectToStringShortTypeConverter();
            AddConverter(typeof(short), shortConverter);
            AddConverter(typeof(short?), shortConverter);

            var floatConverter = new ObjectToStringFloatTypeConverter();
            AddConverter(typeof(float), floatConverter);
            AddConverter(typeof(float?), floatConverter);

            var longConverter = new ObjectToStringLongTypeConverter();
            AddConverter(typeof(long), longConverter);
            AddConverter(typeof(long?), longConverter);

            var dateConverter = new ObjectToStringDateTypeConverter();
            AddConverter(typeof(DateTime), dateConverter);
            AddConverter(typeof(DateTime?), dateConverter);
        }
    }
}
