 # Writing CSV files: Creating your own type converters EXAMPLE 1

Here is the custom type converter included with this project that does NOT have a custom attribute:
```c#
using System;

namespace CsvConverter.ClassToCsv
{
    /// <summary>Trims all fields of white space left and right of the text.</summary>
    public class TrimClassToCsvTypeConverter : IClassToCsvTypeConverter
    {
        public bool CanConvert(Type inputType)
        {
            return inputType == typeof(string);
        }
 
        public CsvConverterTypeEnum ConverterType => CsvConverterTypeEnum.ClassToCsvType;

        public int Order { get; set; } = 999;

        public string Convert(Type inputType, object value, string stringFormat, string columnName, int columnIndex, 
            int rowNumber, IDefaultObjectToStringTypeConverterManager defaultConverters)
        {
            if (value == null)
                return null;

            var theString =  value.ToString();

            return theString.Trim();
        }

        public void Initialize(CsvConverterCustomAttribute attribute)
        {
        }
    }
}
```

Notes:
- In the CanHandleThisInputType method, we tell the system what this conveter can handle so that people don't use it on the wrong property.
- In the Initialize method, nothing is going on since it doesn't need anything from the attribute.

Usage:
```c#
public class WriterAttributeTestExample
{
    public int Month { get; set; }

    public int Age { get; set; }

    [CsvConverterCustom(typeof(TrimClassToCsvTypeConverter))]
    public string Name { get; set; }
}
```