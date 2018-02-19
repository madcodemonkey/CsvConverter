 # Writing CSV files: Creating your own type converters

Custom type converters can be created to handle different types **or**  to make a tweeks to strings before passing them to the default converter.  If your custom converter needs some special inputs (e.g., two integers) you may want to start out by creating a new attribute and then creating a converter to pass to it.

---
## Example 1 (Converter only)
Here is the custom converter included with this project that does NOT have a custom attribute:
```c#
/// <summary>Trims all fields of white space left and right of the text.</summary>
public class TrimClassToCsvTypeConverter : IClassToCsvTypeConverter
{
    public bool CanHandleThisInputType(Type inputType)
    {
        return inputType == typeof(string);
    }

    public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
        int rowNumber, IObjectToStringDefaultConverters defaultConverters)
    {
        if (value == null)
            return null;

        var theString =  value.ToString();

        return theString.Trim();
    }

    public void Initialize(ClassToCsvTypeConverterAttribute attribute)
    {

    }
}
```

Notes:
- In the CanHandleThisInputType method, we tell the system what this conveter can handle so that people don't use it on the wrong property.
- In the Initialize method, nothing is going on since it doesn't need anything from the attribute.

Usage:
```c#
internal class CoverterTypeMismatchTestExample
{
    public int Month { get; set; }

    [ClassToCsvTypeConverter(typeof(TrimClassToCsvTypeConverter))]
    public int Age { get; set; }

    public string Name { get; set; }
}
```

---
## Example 2 (create an attribute AND a converter)

### Creating a custom attribute for your custom converter
In order for the reflection logic to pick up your custom attribute, your attributes must inherit from ClassToCsvTypeConverterAttribute
```C#
using CsvConverter.ClassToCsv;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverterAttribute : ClassToCsvTypeConverterAttribute
    {
        public MoneyFormatterClassToCsvTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }

        public string Format { get; set; } = "C";
    }
}
```

### Custom Converter
All custom converters must implement the IClassToCsvTypeConverter interface, which is defined as follows:
```C#
public interface IClassToCsvTypeConverter
{
    bool CanHandleThisInputType(Type inputType);
    string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber,
        IObjectToStringDefaultConverters defaultConverters);
    void Initialize(ClassToCsvTypeConverterAttribute attribute);
}
```
So we could create a custom converter that looks like this:
```c#
using CsvConverter.ClassToCsv;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverter: IClassToCsvTypeConverter
    {
        private string _formatString;

        public bool CanHandleThisInputType(Type inputType)
        {
            return inputType == typeof(decimal) || inputType == typeof(decimal?) ||
                inputType == typeof(double) || inputType == typeof(double);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IObjectToStringDefaultConverters defaultConverters)
        {
            if (value == null)
                return null;

            if (inputType == typeof(double))
                return ((double)value).ToString(_formatString);

            return ((decimal)value).ToString(_formatString);            
        }

        public void Initialize(ClassToCsvTypeConverterAttribute attribute)
        {
            MoneyFormatterClassToCsvTypeConverterAttribute myAttribute = attribute as MoneyFormatterClassToCsvTypeConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(MoneyFormatterClassToCsvTypeConverterAttribute)} attribute with this converter!");

            _formatString = myAttribute.Format;
        }
    }
}
```

Notes:
- In the CanHandleThisInputType method, we tell the system what this conveter can handle so that people don't use it on the wrong property.
- In the Initialize method, we check that the attribute being passed in is our custom one.
- In the Convert method, we do our work.

Usage:
```C#
using CsvConverter.CsvToClass;

namespace AdvExample1
{
    public class Car
    {
        [TextLengthEnforcerPreprocessor(MaximumLength = 8, MinimumLength = 5, CharacterToAddToShortStrings = '*')]
        public string Model { get; set; }

        [TextLengthEnforcerPreprocessor(MaximumLength = 6, MinimumLength = 4, CharacterToAddToShortStrings = '~')]
        public string Make { get; set; }

        public int Year { get; set; }

        [MoneyFormatterClassToCsvTypeConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format = "C2")]
        [CsvToClassTypeConverter(typeof(MoneyTypeConverter))]
        public decimal PurchasePrice { get; set; }
        
        [MoneyFormatterClassToCsvTypeConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format ="C2")] // TO CSV
        [CsvToClassTypeConverter(typeof(MoneyTypeConverter))] // CSV to Object
        public double CurrentValue { get; set; }

        public override string ToString()
        {
            return string.Format("Model: {0} Make: {1} Year: {2} PurchasePrice {3}  CurrentValue {4}",
                Model,
                Make,
                Year,
                PurchasePrice,
                CurrentValue);
        }
    }
}
```