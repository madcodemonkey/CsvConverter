# CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue
This converter replaces any null or white spaced string, with a new value.

## Which attribute do I use?
It should be used with the special *CsvConverterStringOldAndNewAttribute* attribute.

## Example 
```c#
public class AnimalData
{
    public int Order { get; set; }

    [CsvConverterString(ColumnName = "Animal Type")]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue),
        NewValue = "Unknown", Order = 1, IsPreConverter = true)]
    public string AnimalType { get; set; }
}

OR

[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue),
    TargetPropertyType = typeof(string), NewValue = "Unknown", Order = 1, IsPreConverter = true)]
public class AnimalData
{
    public int Order { get; set; }

    [CsvConverterString(ColumnName = "Animal Type")]
    public string AnimalType { get; set; }
}
```

Notes
- You can decorate the class with pre-converters IF you specify a TargetPropertyType.
