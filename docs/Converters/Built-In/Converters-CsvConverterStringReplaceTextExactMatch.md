# CsvConverterStringReplaceTextExactMatch 
This converter replaces the text specified in the OldValue attribute with text in the NewValue attribute if and only if it is an EXACT match.

## Which attribute do I use?
It should be used with the special *CsvConverterStringOldAndNewAttribute* attribute.

## Example 

```c#
public class AnimalData
{
    public int Order { get; set; }

    [CsvConverterString(ColumnName = "Animal Type")]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
        OldValue = "dog", NewValue = "cat", Order = 1, IsPreConverter = true)]
    public string AnimalType { get; set; }
}

OR

[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch),
    TargetPropertyType = typeof(string), OldValue = "dog", NewValue = "cat", Order = 2, IsPreConverter = true)]
public class AnimalData
{
    public int Order { get; set; }

    [CsvConverterString(ColumnName = "Animal Type")]
    public string AnimalType { get; set; }
}
```

Notes
- You can decorate the class with pre-converters IF you specify a TargetPropertyType.
