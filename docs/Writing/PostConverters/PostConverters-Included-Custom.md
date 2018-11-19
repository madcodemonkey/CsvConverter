 # Writing CSV files: Post-converters included with this library

Post processes a string after it comes out of a type converter, but before it is written to the CSV file.

There are a few custom post-converters included with this project that you can use:
- **ReplaceTextEveryMatchClassToCsvPostConverter** - All instances of the old value are replaced with the new value.
    - Should be used with the default *CsvConverterOldAndNewValueAttribute* attribute
- **ReplaceTextExactMatchClassToCsvPostConverter** - If the input string matches the old value exactly, it is replaced entirely with the new value.
    - Should be used with the special *CsvConverterOldAndNewValueAttribute* attribute.

To use them, decorate the class property the proper attribute (see above):

```c#
internal class ClassToCsvServicePostData1
{
    [CsvConverter(ColumnIndex = 1)]
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
    public int Order { get; set; }

    [CsvConverter(ColumnIndex = 2)]
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
    public int Age { get; set; }

    [CsvConverter(ColumnIndex = 3)]
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "Ch", NewValue = "D")]
    public string Name { get; set; }
}

OR 

[CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null, TargetPropertyType = typeof(int), Order = 1)]
[CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "5", NewValue = null, TargetPropertyType = typeof(int), Order = 2)]
internal class ClassToCsvServicePostData2
{
    [CsvConverter(ColumnIndex = 1)]
    public int Order { get; set; }

    [CsvConverter(ColumnIndex = 2)]
    public int Age { get; set; }

    [CsvConverter(ColumnIndex = 3)]
    public string Name { get; set; }
}
```

Notes
- You can decorate the class with post-converters IF you specify a TargetPropertyType.