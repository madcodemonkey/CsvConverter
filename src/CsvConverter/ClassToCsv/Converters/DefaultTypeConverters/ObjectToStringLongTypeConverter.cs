﻿using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringLongTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(long) || inputType == typeof(long?);
        }
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            long data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (long?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (long)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}