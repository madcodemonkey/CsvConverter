# CsvConverterStringReplaceTextEveryMatch
This converter replaces the text specified in the OldValue attribute with text in the NewValue attribute everywhere it can be found in the string.

## Which attribute do I use?
It should be used with the special *CsvConverterStringOldAndNewAttribute* attribute.

## Example 
```c#
public class AnimalData
{
    public int Order { get; set; }

     // As a PRE-converter using the default TYPE converter
    [CsvConverterString(ColumnName = "Animal Type")]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        OldValue = " ", NewValue = "", Order = 1, IsPreConverter = true)]
    public string AnimalType { get; set; }
}

OR

public class AnimalData
{
    public int Order { get; set; }
   
    // As a TYPE converter
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
        ColumnName = "Animal Type",  OldValue = " ", NewValue = "")]
    public string AnimalType { get; set; }
}

OR

[CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch),
    TargetPropertyType = typeof(string), OldValue = " ", NewValue = "", Order = 1, IsPreConverter = true)]
public class AnimalData
{
    public int Order { get; set; }

    [CsvConverterString(ColumnName = "Animal Type")]
    public string AnimalType { get; set; }
}
```

Notes
- You can decorate the class with pre-converters IF you specify a TargetPropertyType.