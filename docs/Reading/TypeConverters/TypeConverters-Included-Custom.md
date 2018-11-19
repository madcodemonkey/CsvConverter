# Reading CSV files: Custom type converters included

There are a few custom type converters included with this project that you can use:
- **CommaDelimitedIntArrayCsvToClassConverter** - Turns a comma delimited array of integers into an int array or throws an exception if they cannot be parsed.
	- Uses the CsvConverterCustomAttribute attribute
- **DecimalToIntCsvToClassConverter** - Converts a string to a decimal value and then rounds it to the nearest integer.
	- Can use CsvConverterCustomAttribute (if you are ok with Math.Round MidpointRounding.AwayFromZero setting) or you can change the settings using the CsvConverterMathRoundingAttribute
- **PercentCsvToClassConverter** - Turns a string that is a percentage into a decimal value or throws an exception if the conversion fails.
	- Uses the CsvConverterCustomAttribute attribute

To use them, decorate the class property with attribute mentioned above:

```c#
public class CsvServiceConverterTestClass
{
	public int Order { get; set; }
	
	[CsvConverterCustom(typeof(PercentCsvToClassConverter))]
	public decimal Percentage { get; set; }
}
```
