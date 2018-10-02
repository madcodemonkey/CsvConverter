using System;
using System.Collections.Generic;

namespace CsvConverter
{
    /// <summary>Creates type converters</summary>
    public class DefaultTypeConverterFactory : IDefaultTypeConverterFactory
    {
        /// <summary>constructor</summary>
        public DefaultTypeConverterFactory()
        {
            RegisterBuiltInDefaultConverters();
        }

        /// <summary>Creates the appropriate converter for the class property type</summary>
        /// <param name="theClassPropertyType">The property's type.  This is the class property.</param>
        /// <returns>A type converter</returns>
        public ICsvConverter CreateConverter(Type theClassPropertyType)
        {
            if (_converters.ContainsKey(theClassPropertyType))
            {
                var typeToCreate = _converters[theClassPropertyType];
                return (ICsvConverter)Activator.CreateInstance(typeToCreate);
            }
            else
            {
                throw new ArgumentException($"The {nameof(DefaultTypeConverterFactory)} does not contain the {theClassPropertyType.Name} type.");
            }
        }

        /// <summary>Adds a converter to the factory.</summary>
        public void AddConverter(Type theType, Type converterType)
        {
            _converters.Add(theType, converterType);
        }

        /// <summary>Finds a converter in the factory.</summary>
        public T FindConverter<T>(Type theType) where T : class
        {
            if (ConverterExists(theType) == false)
                return null;

            return _converters[theType] as T;
        }

        /// <summary>Finds the type of the converter in the factory and returns it.</summary>
        public Type FindConverterType(Type theType)
        {
            if (ConverterExists(theType) == false)
                return null;

            return _converters[theType];
        }

        /// <summary>Removes a type converter from the factory.</summary>
        public void RemoveConverter(Type theType)
        {
            _converters.Remove(theType);
        }

        /// <summary>Indicates if a type converter exists in the factory.</summary>
        public bool ConverterExists(Type theType)
        {
            return _converters.ContainsKey(theType);
        }

        private Dictionary<Type, Type> _converters = new Dictionary<Type, Type>();

        private void RegisterBuiltInDefaultConverters()
        {
            AddConverter(typeof(int), typeof(CsvConverterDefaultInt));
            AddConverter(typeof(int?), typeof(CsvConverterDefaultInt));

            AddConverter(typeof(decimal), typeof(CsvConverterDefaultDecimal));
            AddConverter(typeof(decimal?), typeof(CsvConverterDefaultDecimal));

            AddConverter(typeof(double), typeof(CsvConverterDefaultDouble));
            AddConverter(typeof(double?), typeof(CsvConverterDefaultDouble));

            AddConverter(typeof(byte), typeof(CsvConverterDefaultByte));
            AddConverter(typeof(byte?), typeof(CsvConverterDefaultByte));

            AddConverter(typeof(bool), typeof(CsvConverterDefaultBoolean));
            AddConverter(typeof(bool?), typeof(CsvConverterDefaultBoolean));

            AddConverter(typeof(short), typeof(CsvConverterDefaultShort));
            AddConverter(typeof(short?), typeof(CsvConverterDefaultShort));

            AddConverter(typeof(float), typeof(CsvConverterDefaultFloat));
            AddConverter(typeof(float?), typeof(CsvConverterDefaultFloat));

            AddConverter(typeof(long), typeof(CsvConverterDefaultLong));
            AddConverter(typeof(long?), typeof(CsvConverterDefaultLong));

            AddConverter(typeof(DateTime), typeof(CsvConverterDefaultDateTime));
            AddConverter(typeof(DateTime?), typeof(CsvConverterDefaultDateTime));

            AddConverter(typeof(string), typeof(CsvConverterDefaultString));
        }
    }
}
