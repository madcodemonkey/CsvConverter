# How do you use a string converter as a pre-processor?

A pre-processor is JUST a string converter.  Any converter that inherits from CsvConverterStringBase is considered a string converter and can be used as a pre-processor, type converter or post-processor.

## Rules for pre-processors
- They are just string type converters that inherit from CsvConverterStringBase (Note: CsvConverterStringBase inherits from CsvConverterTypeBase).
- They must be used with the CsvConverterStringAttribute attribute OR an attribute that inherits from CsvConverterStringAttribute (e.g., CsvConverterStringTrimAttribute).  The reason for this requirement is that the CsvConverterStringAttribute has a IsPreConverter property that must be set to TRUE to indicate that you want a pre-processor. 
- When reading, you may have as many as you wish.
- They will be executed in the order specified by the Order property on the attribute.  If you are only using one, don't bother specifying the Order.  However, you should always specify order if you are using more than one pre-converters on a property.
- They are executed BEFORE type conversion takes place so that they are dealing with strings coming directly from the CSV file OR from the pre-converter that cam before it.

## Example 1
```c#
public class Squirrel
{
	// The CsvConverterStringTrimmer will only be run when reading a file. 
	[CsvConverterString(typeof(CsvConverterStringTrimmer), IsPreConverter = true)]
	public string Name { get; set; }

	// The CsvConverterStringTrimmer will run when reading OR writing a file
	[CsvConverterString(typeof(CsvConverterStringTrimmer))]
	public string Species { get; set; }

	// The CsvConverterStringTrimmer will run when writing a file
	[CsvConverterString(typeof(CsvConverterStringTrimmer), IsPostConverter = true)]
	public string HairColor { get; set; }
	
	// The number 2 will be written as "two" (see the file)
	[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), NewValue = "2", OldValue = "two", IsPreConverter = true)]
	[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), NewValue = "two", OldValue = "2", IsPostConverter = true)]
	public int Age { get; set; }
}
```

Notes
- This code came from the CsvConverter.SimpleCoreExample2 example.
- We are trimming the Name CSV column left and right before passing the resulting string to the default string converter that will assign the string to the Name property.
- The Species CSV column conversion is also interesting because CsvConverterStringTrimmer is used as a TYPE converter instead.  The main differences between Name and Species is that Name is trimmed only when reading whereas Species is trimmed when reading and writing.
- The Age CSV column contains number accept for the 2's which some odd ball wrote as the word "two".  Here the pre-converter converters the word "two" into a "2" so that that the default number converter can properly convert it to a number.

## Example 2
```c#
// EVERY match removes spaces so and EXACT match replaces words.  So ORDER MATTERS here so the exact match and find words.
[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
	TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 1, IsPreConverter = true)]
[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
	TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 2, IsCaseSensitive = false, IsPreConverter = true)]
public class Animal
{
	public int Order { get; set; }

	[CsvConverterString(ColumnName = "Animal Type")]
	public string AnimalType { get; set; }
}
```

Notes
- In this example there are two pre-processors executing against the AnimalType property.  The first removes all spaces and the second converters the world "dog" to "cat" if the entire word matches.  
-  Notice that the TargetPropertyType must be specified.  The reason is that any type converter can be placed on the class so you must tell CsvConveter what you are targetting.
- The order that these to pre-processors execute really matters.  That why removing the spaces is done first (Order = 1) and then the match is done second.
