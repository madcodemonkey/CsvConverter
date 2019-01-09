﻿using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    public class StringToObjectByteTypeConverter : StringToObjectBaseTypeConverter, ICsvToClassTypeConverter
    {
        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(byte) || outputType == typeof(byte?);
        }

        public object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (targetType.HelpIsNullable())
                    return null;

                return (byte)0;
            }

            if (byte.TryParse(stringValue, out byte number))
            {
                return number;
            }

            ThrowCannotConvertError(targetType, stringValue, columnName, columnIndex, rowNumber);
            return (byte)0; // never reached
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            // Nothing on the attribute is needed
        }
    }
}