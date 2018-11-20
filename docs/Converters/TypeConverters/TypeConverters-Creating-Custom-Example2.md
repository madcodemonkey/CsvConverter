# Writing CSV files: Creating your own type converters EXAMPLE 2

## Creating a custom attribute for your custom type converter
In order for the reflection logic to pick up your custom attribute, your attributes must inherit from CsvConverterCustomAttribute
```C#
using CsvConverter;
using System;

namespace AdvExample1
{
    public class MoneyFormatterConverterAttribute : CsvConverterCustomAttribute
    {
        public MoneyFormatterConverterAttribute(Type typeConverter) : base(typeConverter) { }

        public string Format { get; set; } = "C";
    }
}
```

Notes
- CsvConverterCustomAttribute is used by both Csv to Class and Class to Csv converters so keep that in mind when creating and naming your attributes since they could be used for both.

## Custom Converter
All custom converters must implement the IClassToCsvTypeConverter interface, which is defined as follows:
```C#
public interface IClassToCsvTypeConverter : ICsvConverter
{
    bool CanConvert(Type inputType);
    string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber,
        IDefaultObjectToStringTypeConverterManager defaultConverters);
}
```
and

```C#
public interface ICsvConverter
{
    CsvConverterTypeEnum ConverterType { get; }
    int Order { get; }
    void Initialize(CsvConverterCustomAttribute attribute);
}
```

So we could create a custom type converter that looks like this:
```c#
using CsvConverter;
using CsvConverter.ClassToCsv;
using System;

namespace AdvExample1
{
    public class MoneyFormatterClassToCsvTypeConverter: IClassToCsvTypeConverter
    {
        private string _formatString;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order => 999;

        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(decimal) || inputType == typeof(decimal?) ||
                inputType == typeof(double) || inputType == typeof(double);
        }

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            if (value == null)
                return null;

            if (inputType == typeof(double))
                return ((double)value).ToString(_formatString);

            return ((decimal)value).ToString(_formatString);            
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            MoneyFormatterConverterAttribute myAttribute = attribute as MoneyFormatterConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(MoneyFormatterConverterAttribute)} attribute with this converter!");

            _formatString = myAttribute.Format;
        }
    }
}
```

Notes:
- In the CanHandleThisInputType method, we tell the system what this converter can handle so that people don't use it on the wrong property.
- In the Initialize method, we check that the attribute being passed in is our custom one.
- In the Convert method, we do our work.

Usage:
```C#
using CsvConverter;

namespace AdvExample1
{
    public class Car
    {
        [TextLengthEnforcerConverter(MaximumLength = 8, MinimumLength = 5, CharacterToAddToShortStrings = '*')]
        public string Model { get; set; }

        [TextLengthEnforcerConverter(MaximumLength = 6, MinimumLength = 4, CharacterToAddToShortStrings = '~')]
        public string Make { get; set; }

        public int Year { get; set; }

        [MoneyFormatterConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format = "C2")]
        [CsvConverterCustom(typeof(MoneyTypeConverter))]
        public decimal PurchasePrice { get; set; }
        
        [MoneyFormatterConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format ="C2")]
        [CsvConverterCustom(typeof(MoneyTypeConverter))]
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