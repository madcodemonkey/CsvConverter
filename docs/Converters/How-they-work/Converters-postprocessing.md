# How do you use a string converter as a post-processor?

A post-processor is JUST a string converter.  Any converter that inherits from CsvConverterStringBase is considered a string converter and can be used as a pre-processor, type converter or post-processor.

## Rules for pre-processors
- They are just string type converters that inherit from CsvConverterStringBase (Note: CsvConverterStringBase inherits from CsvConverterTypeBase).
- They must be used with the CsvConverterStringAttribute attribute OR an attribute that inherits from CsvConverterStringAttribute (e.g., CsvConverterStringTrimAttribute).  The reason for this requirement is that the CsvConverterStringAttribute has a IsPostConverter property that must be set to TRUE to indicate that you want a post-processor. 
- When writing, you may have as many as you wish.
- They will be executed in the order specified by the Order property on the attribute.  If you are only using one, don't bother specifying the Order.  However, you should always specify order if you are using more than one post-converters on a property.
- They are executed AFTER the type converting is done so that your only dealing with strings.

## Example 1
```c#
public class Animal
{
	[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
      OldValue = " ", NewValue = "", Order = 1, IsPostConverter = true)]
	[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
	   OldValue = "dog", NewValue = "cat", Order = 2, IsPostConverter = true)]
	public string AnimalType { get; set; }
}
```

Notes
- Prior to writing data to the file, we will first remove every space. Afterwards, we will replace exact matches of the word "dog" with the word "cat"
- 

## Example 2
```c#
[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
	TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 1, IsPostConverter = true)]
[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
	TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 2, IsPostConverter = true)]
internal class Animal
{
	public string AnimalType { get; set; }

    public string AnotherString { get; set; }
}
```

Notes
- Exampel 2, will effectively do the same thing as Example 1; however, it will target both AnimalType and AnotherString.