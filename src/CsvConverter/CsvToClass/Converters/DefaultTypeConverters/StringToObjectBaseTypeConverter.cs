using System;
using System.Globalization;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    /// <summary>Base class of default type converters.</summary>
    public abstract class StringToObjectBaseTypeConverter
    {
        /// <summary>Specifies the type of converter.  Since converters of different types (csv to class or class to csv)
        /// can use any attribute that inherits from CsvConverterCustomAttribute, we need some way to identify WHAT a 
        /// converter is doing and that's the purpose of this property.</summary>
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        /// <summary>This is used by converters that are NOT TYPE converters. In other words, we use this for pre and post converters
        /// to determine the order they should be run.  In a converters Initialize method, the  CsvConverterCustomAttribute's Order is
        /// usually assigned to this property.  See the TextReplacerCsvToClassPreConverter's Initialize method for an example of it being used.</summary>
        public int Order => 999;

        /// <summary>Gives a detailed error message if there is a type conversion issue.</summary>
        protected void ThrowCannotConvertError(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber)
        {
            throw new ArgumentException($"The {nameof(DefaultStringToObjectTypeConverterManager)} converter cannot parse the string " +
              $"'{stringValue}' as a {theType.HelpTypeToString()} on row number {rowNumber} in " +
              $"column {columnName} at column index {columnIndex}.");
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

    }
}
