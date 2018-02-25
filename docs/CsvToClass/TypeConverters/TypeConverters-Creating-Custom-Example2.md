# Reading CSV files:  Creating your own custom type converter EXAMPLE 2

## Creating a custom attribute for your custom converter
In order for the reflection logic to pick up your custom attribute, your attributes must inherit from CsvConverterCustomAttribute
```C#
using System;
using CsvConverter;

namespace AdvExample1
{
    public class NumericRangeTypeConverterAttribute : CsvConverterCustomAttribute
    {
        public NumericRangeTypeConverterAttribute(Type typeConverter) : base(typeConverter) { }
        public int Minimum { get; set; } = 1;
        public int Maximum { get; set; } = 20;
    }
}
```

Notes
- CsvConverterCustomAttribute is used by both Csv to Class and Class to Csv converters so keep that in mind when creating and naming your attributes since they could be used for both.


## Custom Converter
All custom converters must implement the ICsvToClassTypeConverter interface, which is defined as follows:
```C#
public interface ICsvToClassTypeConverter : ICsvConverter
{
	bool CanConvert(Type outputType);
	object Convert(Type targetType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverters);	
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
using CsvConverter;
using CsvConverter.CsvToClass;

namespace AdvExample1
{
    public class NumericRangeTypeConverter : ICsvToClassTypeConverter
    {
        private int _minimum = 0;
        private int _maximum = 20;

        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        public int Order => 999;

        public bool CanConvert(Type outputType)
        {
            return outputType == typeof(int) || outputType == typeof(int?);
        }

        public object Convert(Type outputType, string stringValue, string columnName,
            int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverter)
        {
            var data = defaultConverter.Convert(outputType, stringValue, columnName, columnIndex, rowNumber);
            if (data == null)
                return data;

            int dataAsNumber = (int)data;
            if (dataAsNumber < _minimum)
                dataAsNumber = _minimum;
            else if (dataAsNumber > _maximum)
                dataAsNumber = _maximum;


            return dataAsNumber;
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
            NumericRangeTypeConverterAttribute myAttribute = attribute as NumericRangeTypeConverterAttribute;
            if (myAttribute == null)
                throw new ArgumentException($"Please use the {nameof(NumericRangeTypeConverterAttribute)} attribute with this converter!");

            _minimum = myAttribute.Minimum;
            _maximum = myAttribute.Maximum;
        }

    }
}
```

Notes:
- In the CanConvert method, we tell the system what this converter can handle so that people don't use it on the wrong property.
- In the Initialize method, we check that the attribute being passed in is our custom one.
- In the Convert method, we let the default converter do all the work.  If it gives us a null, we give it the default; otherwise, we check the range and adjust it if necessary.

Usage:
```C#
using CsvConverter;

namespace AdvExample1
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NumericRangeTypeConverter(typeof(NumericRangeTypeConverter), Minimum = 1, Maximum = 50)]
        public int Age { get; set; }

        public decimal PercentageBodyFat { get; set; }
        public double AvgHeartRate { get; set; }

        [CsvConverter(IgnoreWhenReading = true, IgnoreWhenWriting = true)]
        public Person Parent { get; set; }
    }
}
```