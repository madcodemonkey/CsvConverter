# Reading CSV files: Custom  pre-converters included with this library

There are a few custom pre-converters included with this project that you can use:
- StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter - If the CSV field contains a null OR blank text, it changes it into a null. 
    - Should be used with the CsvConverterCustomAttribute attribute
- TextReplacerCsvToClassPreConverter - Replaces the text specified in the OldValue attribute with text in the NewValue attribute
    - Should be used with the CsvConverterOldAndNewValueAttribute attribute.
- TrimCsvToClassPreConverter - Trims all fields of white space left and right of the text.
    - Should be used with the CsvConverterCustomAttribute attribute

To use them, decorate the class property the proper attribute (see above).:

```c#
public class CsvServiceMultipleOnPropPreConverterTestClass
{
    public int Order { get; set; }

    [CsvConverterCustom(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter), Order = 1)]
    [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), Order = 2)]
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

OR

public class ReaderAttributeTestExample
{
    [CsvConverterCustom(typeof(DecimalToIntCsvToClassConverter))]
    public int Month { get; set; }

    public int Age { get; set; }

    [CsvConverterOldAndNewValue(typeof(TextReplacerCsvToClassPreConverter), OldValue ="#", NewValue ="", Order = 1)]
    [CsvConverterCustom(typeof(TrimCsvToClassPreConverter), Order = 2)]
    public string Name { get; set; }
}

OR


[CsvConverterCustom(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter), TargetPropertyType = typeof(string), Order = 1)]
[CsvConverterCustom(typeof(TrimCsvToClassPreConverter), TargetPropertyType = typeof(string), Order = 2)]
public class CsvServiceMultipleOnClassPreConverterTestClass
{
    public int Order { get; set; }
    public string FirstName { get; set; }      
    public string LastName { get; set; }
}
```

Notes
- You can decorate the class with pre-converters IF you specify a TargetPropertyType.