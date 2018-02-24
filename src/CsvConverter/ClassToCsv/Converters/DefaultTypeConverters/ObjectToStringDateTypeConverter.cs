﻿using CsvConverter.Shared;
using System;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringDateTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(DateTime) || inputType == typeof(DateTime?);
        }

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            DateTime data;
            if (inputType.HelpIsNullable())
            {
                var rawNull = (DateTime?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (DateTime)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
