using System;

namespace CsvConverter.CsvToClass
{
    /// <summary>Turns a comma delimited array of integers into an int array or throws an exception if they cannot be parsed.</summary>
    public class CommaDelimitedIntArrayCsvToClassConverter : ICsvToClassTypeConverter
    {
        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(int[]);
        }
             
        public object Convert(Type outputType, string stringValue, string columnName, int columnIndex, int rowNumber, IStringToObjectDefaultConverters defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
               return null;
            }

            int[] result = null;

            if (stringValue != null)
            {
                string[] source = stringValue.Split(',');
                result = new int[source.Length];
                for (int index = 0; index < source.Length; index++)
                {
                    if (int.TryParse(source[index], out result[index]) == false)
                    {
                        throw new ArgumentException($"The {nameof(CommaDelimitedIntArrayCsvToClassConverter)} converter cannot parse the '{stringValue}' string.  " +
                            $"The value at index {index} is is not an integer: '{source[index]}' on row number {rowNumber}.");
                    }
                }
            }

            return result;
        }

     
        public void Initialize(CsvToClassTypeConverterAttribute attribute)
        {
            
        }
    }

}
