using CsvConverter;
using System;

namespace AdvExample2
{
    public class CsvConverterStringTextLengthEnforcer : CsvConverterStringBase, ICsvConverterString
    {
        public char CharacterToAddToShortStrings { get; set; } = '*';
        public int MaximumLength { get; set; } = 10;
        public int MinimumLength { get; set; } = 1;

        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            if (value == null)
                return null;

            return EnforceMinimumLength((string)value);
        }

        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            return EnforceMinimumLength(value);
        }

        private string EnforceMinimumLength(string value)
        {
            if (value != null)
            {
                if (value.Length < MinimumLength)
                {
                    while (value.Length < MinimumLength)
                        value += CharacterToAddToShortStrings;

                }
                else if (value.Length > MaximumLength)
                {
                    value = value.Substring(0, MaximumLength);
                }
            }

            return value;
        }
       

    }
}
