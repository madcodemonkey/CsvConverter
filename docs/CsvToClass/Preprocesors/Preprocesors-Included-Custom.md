# Reading CSV files: Custom  pre-processors included

There are a few custom pre-processors included with this project that you can use:
- StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor - If the CSV field contains a null OR blank text, it changes it into a null. 
- TextReplacerCsvToClassPreprocessor - Replaces the text specified in the OldValue attribute with text in the NewValue attribute
    - Should be used with the CsvToClassNewAndOldValuePreprocessorAttribute attribute.
- TrimCsvToClassPreprocessor - Trims all fields of white space left and right of the text.

To use them, decorate the class property with the CsvToClassPreprocessorAttribute:

```c#
[CsvToClassPreprocessor(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor), TargetPropertyType = typeof(string))]
internal class CsvServiceMultipleOnClassPreprocessorTestClass
{
    public int Order { get; set; }
    public string FirstName { get; set; }      
    public string LastName { get; set; }
}

OR

internal class CsvServiceMultipleOnPropPreprocessorTestClass
{
    public int Order { get; set; }

    [CsvToClassPreprocessor(typeof(StringIsNullOrWhiteSpaceSetToNullCsvToClassPreprocessor))]
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```