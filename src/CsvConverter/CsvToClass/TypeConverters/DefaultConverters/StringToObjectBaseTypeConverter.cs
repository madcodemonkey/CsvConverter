using System;
using System.Globalization;

namespace CsvConverter.CsvToClass
{
    public abstract class StringToObjectBaseTypeConverter
    {
        protected void ThrowCannotConvertError(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber)
        {
            throw new ArgumentException($"The {nameof(StringToObjectConverter)} converter cannot parse the string " +
              $"'{stringValue}' as a {theType.Name} on row number {rowNumber} in " +
              $"column {columnName} at column index {columnIndex}.");
        }

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
