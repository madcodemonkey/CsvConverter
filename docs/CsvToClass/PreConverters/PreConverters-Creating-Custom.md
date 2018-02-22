# Reading CSV files:  Creating your own pre-converters

Custom type pre-converters can be created to process the CSV field string BEFORE they reach a converter.  This can be handy if you know there is something in the field that may not convert directly to a type.  For example, removing single quotes from around a number so that it can be converted to a number without error.  You may also want to create a custom attribute if you need special inputs for your converter. 

---
## Example 1 (create an attribute AND a converter)

### Creating a custom attribute for your custom converter
In order for the reflection logic to pick up your custom attribute, your attributes must inherit from CsvConverterCustomAttribute
```C#
using CsvConverter;

namespace AdvExample1
{
    public class TextLengthEnforcerConverterAttribute : CsvConverterCustomAttribute
    {
        public TextLengthEnforcerConverterAttribute() : base(typeof(TextLengthEnforcerCsvToClassPreConverter)) { }

        public char CharacterToAddToShortStrings { get; set; } = '~';
        public int MaximumLength { get; set; } = int.MaxValue;
        public int MinimumLength { get; set; } = int.MinValue;
    }
}
```

### Custom Converter
All custom converters must implement the ICsvToClassPreConverter interface, which is defined as follows:
```C#
public interface ICsvToClassPreConverter : ICsvConverter
{
    bool CanProcessType(Type theType);
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
using CsvConverter;
using CsvConverter.CsvToClass;
using System;

namespace AdvExample1
{
    public class TextLengthEnforcerCsvToClassPreConverter : ICsvToClassPreConverter
    {
        private char _characterToAddToShortStrings;
        private int _maximumLength;
        private int _minimumLength;

        public int Order { get; set; } = 999;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassPre;

        public bool CanProcessType(Type theType)
        {
            return theType == typeof(string);
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            var myAttribute = attribute as TextLengthEnforcerConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(TextLengthEnforcerConverterAttribute)} attribute with this pre-converter!");

            _maximumLength = myAttribute.MaximumLength;
            _minimumLength = myAttribute.MinimumLength;
            _characterToAddToShortStrings = myAttribute.CharacterToAddToShortStrings;
        }

        public string Convert(string csvField, string columnName, int columnIndex, int rowNumber)
        {
            if (csvField != null)
            {
                if (csvField.Length < _minimumLength)
                {
                    while (csvField.Length < _minimumLength)
                        csvField += _characterToAddToShortStrings;

                }
                else if (csvField.Length > _maximumLength)
                {
                    csvField = csvField.Substring(0, _maximumLength);
                }
            }

            return csvField;
        }
    }
}
```

Notes:
- In the CanProcessType method, we tell the system what this pre-conveter can handle so that people don't use it on the wrong property.
- In the Initialize method, we check that the attribute being passed in is our custom one.
- In the Convert method, we do our string checking.

Usage:
```C#
using CsvConverter;

namespace AdvExample1
{
    public class Person
    {
        [TextLengthEnforcerConverter(typeof(TextLengthEnforcerCsvToClassPreConverter), MinimumLength = 5, MaximumLength = 50)]
        public string FirstName { get; set; }

        [TextLengthEnforcerConverter(typeof(TextLengthEnforcerCsvToClassPreConverter), MinimumLength = 5, MaximumLength = 50)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public decimal PercentageBodyFat { get; set; }

        public double AvgHeartRate { get; set; }        
    }
}
```