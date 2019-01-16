using System;
using System.Text;

namespace CsvConverter
{
    // TODO: Tests needed for this class
    /// <summary>Turns a comma delimited array of integers into an int array or throws an exception if they cannot be parsed.</summary>
    public class CsvConverterCommaDelimitedIntArray : CsvConverterTypeBase, ICsvConverter
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(int[]);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(int[]);
        }

        /// <summary>Converts a string to an int[]</summary>
        public object GetReadData(Type inputType, string value, string columnName, int columnIndex, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            int[] result = null;

            if (value != null)
            {
                string[] source = value.Split(',');
                result = new int[source.Length];
                for (int index = 0; index < source.Length; index++)
                {
                    if (int.TryParse(source[index], out result[index]) == false)
                    {
                        throw new ArgumentException($"The {nameof(CsvConverterCommaDelimitedIntArray)} converter cannot parse the '{value}' string.  " +
                            $"The value at index {index} is is not an integer: '{source[index]}' on row number {rowNumber}.");
                    }
                }
            }

            return result;
        }

        /// <summary>Converts a int[] to a command delimited string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            int[] input = value as int[];
            if (input == null || input.Length == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach(int number in input)
            {
                if (sb.Length > 0)
                    sb.Append(",");
                sb.Append(number.ToString());
            }

            return sb.ToString();
        }
    }
}