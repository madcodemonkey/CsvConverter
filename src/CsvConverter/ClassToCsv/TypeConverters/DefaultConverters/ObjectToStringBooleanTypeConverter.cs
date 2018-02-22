﻿using CsvConverter.Shared;
using CsvConverter.TypeConverters;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringBooleanTypeConverter : IClassToCsvTypeConverter, IBooleanConverterSettings
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(bool) || inputType == typeof(bool?);
        }
        
        public BooleanOutputFormatEnum OutputFormat { get; set; } = BooleanOutputFormatEnum.UseTrueAndFalse;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvConverter;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName,
            int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            bool data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (bool?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (bool)value;
            }

            switch (OutputFormat)
            {
                case BooleanOutputFormatEnum.UseTrueAndFalse:
                    return data ? "True" : "False";
                case BooleanOutputFormatEnum.UseOneAndZero:
                    return data ? "1" : "0";
                case BooleanOutputFormatEnum.UseTandF:
                    return data ? "T" : "F";
                case BooleanOutputFormatEnum.UseYandN:
                    return data ? "Y" : "N";
                case BooleanOutputFormatEnum.UseYesAndNo:
                    return data ? "Yes" : "No";
            }

            return data.ToString();
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
