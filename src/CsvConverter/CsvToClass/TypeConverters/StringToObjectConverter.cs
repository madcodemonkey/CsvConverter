﻿using System;
using System.Collections.Generic;
using CsvConverter.TypeConverters;

namespace CsvConverter.CsvToClass
{
    /// <summary>Converts a string based on the a property's type and assigns the converted value to the property.</summary>
    /// <remarks>I was originally going to write this as an extension and return an object from each call to AssignValue,
    /// but upon considering the thousands of calls that would be made to the AssignValue method I didn't think it 
    /// wise to create that many objects especially when most would be a "good" result and not used.</remarks>
    public class StringToObjectConverter : IStringToObjectConverter
    {
        public StringToObjectConverter()
        {
            RegisterBuiltInConverters();
        }

        public void AddConverter(Type outputType, ICsvToClassTypeConverter converter)
        {
            if (converter == null)
                throw new ArgumentNullException("Please specify a converter.  If you are trying to remove a converter, please use the RemoveConverter method.");

            if (converter.CanOutputThisType(outputType) == false)
                throw new ArgumentException($"The converter cannot handle the {outputType.Name} output type.");

            _converters.Add(outputType, converter);
        }

        public object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber)
        {
            if (theType == null)
                throw new ArgumentNullException("You must specify a type.");

            if (_converters.ContainsKey(theType))
            {
                // Sending in Bogus converter to warn users that DEFAULT converters cannot fallback on a any other converters!
                return _converters[theType].Convert(theType, stringValue, columnName, columnIndex, rowNumber, _bogusConverter);
            }
            else
            {
                throw new ArgumentException("Unable to find a converter in order to convert the data " +
                    $"'{stringValue}' into the data type {theType.Name} on row number {rowNumber} in the " +
                    $"'{columnName}' field at CSV column index {columnIndex}.");
            }
        }


        public ICsvToClassTypeConverter FindConverter(Type theType)
        {
            if (ConverterExists(theType) == false)
                return null;

            return _converters[theType];
        }

        public T FindConverter<T>(Type theType) where T : class
        {
            if (ConverterExists(theType) == false)
                return null;

            return _converters[theType] as T;
        }

        public void RemoveConverter(Type typeToConvert)
        {
            _converters.Remove(typeToConvert);
        }

        public bool ConverterExists(Type typeToConvert)
        {
            return _converters.ContainsKey(typeToConvert);
        }

        private Dictionary<Type, ICsvToClassTypeConverter> _converters = new Dictionary<Type, ICsvToClassTypeConverter>();
        private IStringToObjectConverter _bogusConverter = new BogusStringToObjectConverter();

        private void RegisterBuiltInConverters()
        {
            var boolConverter = new StringToObjectBooleanTypeConverter();
            AddConverter(typeof(bool), boolConverter);
            AddConverter(typeof(bool?), boolConverter);

            var decimalConverter = new StringToObjectDecimalTypeConverter();
            AddConverter(typeof(decimal), decimalConverter);
            AddConverter(typeof(decimal?), decimalConverter);

            var doubleConverter = new StringToObjectDoubleTypeConverter();
            AddConverter(typeof(double), doubleConverter);
            AddConverter(typeof(double?), doubleConverter);

            var intConverter = new StringToObjectIntTypeConverter();
            AddConverter(typeof(int), intConverter);
            AddConverter(typeof(int?), intConverter);

            var byteConverter = new StringToObjectByteTypeConverter();
            AddConverter(typeof(byte), byteConverter);
            AddConverter(typeof(byte?), byteConverter);

            var shortConverter = new StringToObjectShortTypeConverter();
            AddConverter(typeof(short), shortConverter);
            AddConverter(typeof(short?), shortConverter);

            var floatConverter = new StringToObjectFloatTypeConverter();
            AddConverter(typeof(float), floatConverter);
            AddConverter(typeof(float?), floatConverter);

            var longConverter = new StringToObjectLongTypeConverter();
            AddConverter(typeof(long), longConverter);
            AddConverter(typeof(long?), longConverter);

            var dateTimeConverter = new StringToObjectDateTimeTypeConverter();
            AddConverter(typeof(DateTime), dateTimeConverter);
            AddConverter(typeof(DateTime?), dateTimeConverter);

            AddConverter(typeof(string), new StringToObjectStringTypeConverter());
        }


        public void UpdateDateTimeParsing(IDateConverterSettings settings)
        {
            var converter = FindConverter<IDateConverterSettings>(typeof(DateTime));
            if (converter != null)
            {
                converter.DateParseExactFormat = settings.DateParseExactFormat;
                converter.DateFormatProvider = settings.DateFormatProvider;
                converter.DateStyle = settings.DateStyle;
            }
        }

        public void UpdateDoubleSettings(IDoubleConverterSettings settings)
        {
            var converter = FindConverter<IDoubleConverterSettings>(typeof(double));
            if (converter != null)
                converter.NumberOfDecimalPlaces = settings.NumberOfDecimalPlaces;
        }

        public void UpdateDecimalSettings(IDecimalConverterSettings settings)
        {
            var converter = FindConverter<IDecimalConverterSettings>(typeof(decimal));
            if (converter != null)
                converter.NumberOfDecimalPlaces = settings.NumberOfDecimalPlaces;

        }


    }

    #region  Bogus converter
    internal class BogusStringToObjectConverter : IStringToObjectConverter
    {
        public void AddConverter(Type outputType, ICsvToClassTypeConverter converter)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public bool ConverterExists(Type typeToConvert)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public ICsvToClassTypeConverter FindConverter(Type theType)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public T FindConverter<T>(Type theType) where T : class
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void RemoveConverter(Type typeToConvert)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDateTimeParsing(IDateConverterSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDecimalSettings(IDecimalConverterSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }

        public void UpdateDoubleSettings(IDoubleConverterSettings settings)
        {
            throw new ArgumentException("You've replaced the default converter so there is no fallback converter!!!");
        }
    }
    #endregion // Bogus converter
}
