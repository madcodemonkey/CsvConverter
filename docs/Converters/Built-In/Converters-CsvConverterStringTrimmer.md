# CsvConverterStringTrimmer
This converter trims a string with string's TrimStart, TrimEnd or Trim methods.  The default method used is Trim.

## Which attribute do I use?
It should be used with the special *CsvConverterStringTrimAttribute* attribute if you want to specify the trim type; otherwise, it will also work with the *CsvConverterStringAttribute* attribute.

## Example 
```c#
public class CsvConverterStringTrimReadData1
{
	public int Order { get; set; }

	[CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), TrimAction = CsvConverterTrimEnum.TrimEnd)]
	public string SomeText { get; set; }
	public string OtherText { get; set; }
}

[CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), TrimAction = CsvConverterTrimEnum.All,
	TargetPropertyType = typeof(string), IsPreConverter = true)]
public class CsvConverterStringTrimReadData2
{
	public int Order { get; set; }
	public string SomeText { get; set; }
	public string OtherText { get; set; }
}
```

Notes
- You can decorate the class with pre-converters IF you specify a TargetPropertyType.