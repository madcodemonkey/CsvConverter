# Reading CSV files:  Creating your own custom type converter

Custom type converters can be created to handle different types **or**  to make a tweeks to strings before passing them to the default converter.  If your custom converter needs some special inputs (e.g., two integers) you may want to start out by creating a new attribute and then creating a converter to pass to it.

---
## Example 1 (Converter only)
Here is the custom converter included with this project that does NOT have a custom attribute:
```c#
using System;
using CsvConverter.Shared;

namespace CsvConverter.CsvToClass
{
    /// <summary>Turns a string that is a percentage into a decimal value or throws an exception if the conversion fails.</summary>
    public class PercentCsvToClassConverter : ICsvToClassTypeConverter
    {
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.CsvToClassType;

        public int Order => 999;

        public bool CanOutputThisType(Type outputType)
        {
            return outputType == typeof(decimal) || outputType == typeof(decimal?);
        }
        
        public object Convert(Type outputType, string stringValue, string columnName, int columnIndex, int rowNumber, IDefaultStringToObjectTypeConverterManager defaultConverters)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (outputType.HelpIsNullable())
                    return null;

                return 0.0m;
            }

            // If there is a percentage sign attempt to handle it; 
            // otherwise, differ to the default converter.
            int indexOfPercentSign = stringValue.IndexOf("%");
            if (indexOfPercentSign != -1)
            {
                // Remove the percentage sign
                stringValue = stringValue.Remove(indexOfPercentSign);
            }

            // Assign the decimal
            object someData = defaultConverters.Convert(outputType, stringValue, columnName, columnIndex, rowNumber);
            if (someData != null)
            {
                return (decimal)someData / 100.0m;
            }

            throw new ArgumentException($"The {nameof(PercentCsvToClassConverter)} converter cannot parse the string " +
                  $"'{stringValue}' as a {outputType.Name} on row number {rowNumber} in " +
                  $"column {columnName} at column index {columnIndex}.");
        }


        public void Initialize(CsvConverterCustomAttribute attribute)
        {

        }
    }
}
```

Notes:
- In the CanOutputThisType method, we tell the system what this conveter can handle so that people don't use it on the wrong property.
- In the Initialize method, nothing is going on since it doesn't need anything from the attribute.
- In the Convert method, it strips out the % sign and uses the default converter to convert a string into a decimal and then divide by 100.

Usage:
```c#
public class CsvServiceConverterTestClass
{
    public int Order { get; set; }

    
    [CsvConverterCustom(typeof(PercentCsvToClassConverter))]
    public decimal Percentage { get; set; }
}
```

---
## Example 2 (create an attribute AND a converter)

### Creating a custom attribute for your custom converter
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

### Custom Converter
All custom converters must implement the ICsvToClassTypeConverter interface, which is defined as follows:
```C#
public interface ICsvToClassTypeConverter : ICsvConverter
{
	bool CanOutputThisType(Type outputType);
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

        public bool CanOutputThisType(Type outputType)
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
- In the CanOutputThisType method, we tell the system what this conveter can handle so that people don't use it on the wrong property.
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