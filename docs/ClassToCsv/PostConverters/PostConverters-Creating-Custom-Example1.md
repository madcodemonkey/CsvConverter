# Reading CSV files:  Creating your own post-converters EXAMPLE 1

## Creating a custom attribute for your custom post-converter
In order for the reflection logic to pick up your custom attribute, your attributes must inherit from the default CsvConverterCustomAttribute
```C#
using System;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterOldAndNewValueAttribute : CsvConverterCustomAttribute
    {
        public CsvConverterOldAndNewValueAttribute(Type converterType) : base(converterType) { }

        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
```

Notes
- CsvConverterCustomAttribute is used by both Csv to Class and Class to Csv converters so keep that in mind when creating and naming your attributes since they could be used for both.

## Custom Converter
All custom converters must implement the IClassToCsvPostConverter interface, which is defined as follows:
```C#
public interface IClassToCsvPostConverter : ICsvConverter
{
    string Convert(string csvField, string columnName, int columnIndex, int rowNumber);
}
```
and ICsvConverter is defined as follows:
```c#
public interface ICsvConverter
{
    CsvConverterTypeEnum ConverterType { get; }
    int Order { get; }
    void Initialize(CsvConverterCustomAttribute attribute);
}
```

So we could create a custom converter that looks like this:
```c#
using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>If the input string matches the old value exactly, it is replaced entirely with the new value.</summary>
    public class ReplaceTextExactMatchClassToCsvPostConverter : IClassToCsvPostConverter
    {
        private string _newValue;
        private string _oldValue;

        public int Order { get; set; } = 1;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvPost;

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var postProcess = attribute as CsvConverterOldAndNewValueAttribute;
            if (postProcess == null)
                throw new ArgumentException($"Please use the {nameof(CsvConverterOldAndNewValueAttribute)} attribute with this post converter ({nameof(ReplaceTextEveryMatchClassToCsvPostConverter)}).");

            _newValue = postProcess.NewValue;
            _oldValue = postProcess.OldValue;
        }

        public string Convert(string csvField, string columnName, int columnIndex, int rowNumber)
        {            
            if (csvField == _oldValue)
                return _newValue;

            return csvField;
        }
    }
}
```

Notes:
- In the Initialize method, we check that the attribute being passed in is our custom one.
- In the Convert method, we do our string work.

Usage:
```C#
internal class ClassToCsvServicePostData1
{
    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
    public int Order { get; set; }

    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null)]
    public int Age { get; set; }

    [CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "Ch", NewValue = "D")]
    public string Name { get; set; }
}

OR

[CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "0", NewValue = null, TargetPropertyType = typeof(int), Order = 1)]
[CsvConverterOldAndNewValue(typeof(ReplaceTextEveryMatchClassToCsvPostConverter), OldValue = "5", NewValue = null, TargetPropertyType = typeof(int), Order = 2)]
internal class ClassToCsvServicePostData2
{
    public int Order { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
}
```