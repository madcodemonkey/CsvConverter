# How do you create a string converter?

String converters are just like custom converters except you use a different base class.

1. Inherit from the CsvConverterStringBase class (note that CsvConverterStringBase inherits from the CsvConverterTypeBase class, which is required for all converters)
1. Implement the ICsvConverterString interface
1. Override the Initialize method if you need to pull data off the attribute that specified your converter.

## Code
```c#
using System;

namespace CsvConverter
{
    /// <summary>Trims all fields of white space left and right of the text.</summary>
    public class CsvConverterStringTrimmer : CsvConverterStringBase, ICsvConverterString
    {
        /// <summary>Can this converter turn a CSV column string into the property type specifed?</summary>
        /// <param name="propertyType">The type that should be returned from the GetReadData method.</param>
        public bool CanRead(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        /// <summary>Can this converter turn the property type specified into a CSV column string?</summary>
        /// <param name="propertyType">The class property type that you must convert into a string.</param>
        public bool CanWrite(Type propertyType)
        {
            return propertyType == typeof(string);
        }

        public CsvConverterTrimEnum TrimAction { get; set; } = CsvConverterTrimEnum.All;

        public object GetReadData(Type targetType, string value, string columnName, int columnIndex, int rowNumber)
        {
            return TrimTheString(value);
        }

        /// <summary>Converts a string into a trimmed string</summary>
        public string GetWriteData(Type inputType, object value, string columnName, int columnIndex, int rowNumber)
        {
            return TrimTheString(value);
        }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute,
            IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (attribute is CsvConverterStringTrimAttribute oneAttribute)
            {
                TrimAction = oneAttribute.TrimAction;
            }
        }

        private string TrimTheString(object value)
        {
            if (value == null)
                return null;

            var theString = value.ToString();

            switch (TrimAction)
            {
                case CsvConverterTrimEnum.TrimStart:
                    return theString.TrimStart();
                case CsvConverterTrimEnum.TrimEnd:
                    return theString.TrimEnd();
                default:
                    return theString.Trim();
            }
        }
    }

}
```
Notes
- CanRead:  Here is where you indicate if you can convert a string into the specified type.  This is used when reading CSV files.
- CanWrite:  Here is where you indicate if you can convert type into a string.  This is used when wtiting CSV files.
- GetReadData:  Given a string, convert it to the specified type.   
- GetWriteData:  Given a string, trim it according to what the user specified in the attribute (see Initialize method)
- Initialize: Here is where you double check that your converter is being used with the proper attribute.  In this converter, the CsvConverterStringTrimAttribute attribute was NOT necessary; however, if it was specified, use it to determine the trim action.

## Usage
```c#
public class Squirrel
{
	[CsvConverterString(typeof(CsvConverterStringTrimmer), IsPreConverter = true)]
	public string Name { get; set; }

}

OR

public class SomeData
{
    public int Order { get; set; }

    [CsvConverterStringTrim(typeof(CsvConverterStringTrimmer), TrimAction = CsvConverterTrimEnum.TrimEnd)]
    public string SomeText { get; set; }

    public string OtherText { get; set; }
}
```
