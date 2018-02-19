﻿using System;
using CsvConverter.Shared;

namespace CsvConverter.ClassToCsv
{
    public class ObjectToStringByteTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(byte) || inputType == typeof(byte?);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, 
            int columnIndex, int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            byte data;
            if (inputType.HelpIsNullable())                
            {
                var rawNull = (byte?)value;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (byte)value;
            }

            return data.ToString(stringFormat);
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {
           
        }
    }
}