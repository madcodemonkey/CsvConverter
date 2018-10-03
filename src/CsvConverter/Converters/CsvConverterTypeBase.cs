using CsvConverter.Reflection;
using System;
using System.Globalization;

namespace CsvConverter
{

    /// <summary>Base class for all converters.</summary>
    public abstract class CsvConverterTypeBase
    {
        /// <summary>Initializes the converter with an attribute</summary>
        public virtual void Initialize(CsvConverterBaseAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
        }

        /// <summary>This is shared code for the Float, Decimal and Double Type converters to look for possible special characters and return a value that 
        /// each can convert as a string.</summary>
        protected string ConvertSpecialStrings(string stringValue)
        {
            if (stringValue.Contains("%"))
            {
                string stringWithoutPercentageSign = stringValue.Replace("%", "");
                double number;
                if (double.TryParse(stringWithoutPercentageSign, out number) == false)
                    return stringValue;

                number = number / 100.0;

                return number.ToString(CultureInfo.InvariantCulture);
            }

            return stringValue;
        }

        /// <summary>Gives a detailed error message if there is a type conversion issue.</summary>
        /// <param name="theTypeOfTheConverter">The class that inherited from this class</param>
        /// <param name="outputType">Type type being converted</param>
        /// <param name="stringValue">The string input value</param>
        /// <param name="columnName">The column name</param>
        /// <param name="columnIndex">The column's index in the file</param>
        /// <param name="rowNumber">The row number in the file.</param>
        /// <param name="optionalMessage">An optional message.</param>
        protected void ThrowConvertErrorWhileReading(Type theTypeOfTheConverter, Type outputType,
            string stringValue, string columnName, int columnIndex, int rowNumber,
            string optionalMessage = null)
        {
            string message = $"The {theTypeOfTheConverter.HelpTypeToString()} converter cannot parse the string " +
              $"'{stringValue}' as a {outputType.HelpTypeToString()} on row number {rowNumber} in " +
              $"column {columnName} at column index {columnIndex}.";

            // Add optional message to the end?
            if (string.IsNullOrWhiteSpace(optionalMessage) == false)
            {
                message += $"  {optionalMessage}";
            }

            throw new ArgumentException(message);
        }

    }
}
