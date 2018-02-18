# Reading CSV files: Custom type converters included

There are a few custom converters that you can use:
- CommaDelimitedIntArrayCsvToClassConverter - Turns a comma delimited array of integers into an int array or throws an exception if they cannot be parsed.
- DecimalToIntCsvToClassConverter - Converts a string to a decimal value and the rounds it to the nearest integer.
- PercentCsvToClassConverter - Turns a string that is a percentage into a decimal value or throws an exception if the conversion fails.

To use them, decorate the class property with the CsvToClassTypeConverterAttribute:

```c#
internal class CsvServiceConverterMismatchTestClass
{
	public int Order { get; set; }

	[CsvToClassTypeConverter(typeof(PercentCsvToClassConverter))]
	public double Percentage { get; set; }
}
```
