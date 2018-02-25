# Writing CSV files: Custom type converters included

There are a few custom converters included with this project that you can use:
- TrimClassToCsvTypeConverter - Trims all fields of white space left and right of the text

To use them, decorate the class property with the CsvConverterCustomAttribute:

```c#
public class WriterAttributeTestExample
{
	public int Month { get; set; }

	public int Age { get; set; }

	[CsvConverterCustom(typeof(TrimClassToCsvTypeConverter))]
	public string Name { get; set; }
}
```
