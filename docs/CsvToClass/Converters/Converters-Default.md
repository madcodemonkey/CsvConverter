# Reading CSV files: Default type converters

 All default converters are held within the CsvToClassService class DefaultConverters property, which helps it convert a string into an object.  The object is then assigned to a class property.  The DefaultConverters property is called in the GetRecord() method if a custom type converter is not specified and is passed to the custom converter if one is specified.

IStringToObjectDefaultConverters is defined as follows:
```C#
public interface IStringToObjectDefaultConverters
{
	void AddConverter(Type outputType, ICsvToClassTypeConverter converter);
	object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber);
	ICsvToClassTypeConverter FindConverter(Type theType);
	T FindConverter<T>(Type theType) where T : class;
	bool ConverterExists(Type typeToConvert);
	void RemoveConverter(Type typeToConvert);
	void UpdateDoubleSettings(IDoubleConverterSettings settings);
	void UpdateDecimalSettings(IDecimalConverterSettings settings);
	void UpdateDateTimeParsing(IDateConverterSettings settings);
}
```

Notes
- You can add and remove converters using AddConverter and RemoveConverter.
- These converters are type specific and will be called for a given type when a custom converter is NOT specified.
- You could add a type converter for non-privitive types (Class properties, arrays, etc.). For example, if you want it to handle typeof(int[]), you could add a new converter to handle it. 

## Default type converters
If you do NOT specify a type converter for a property, one of these default converters will used:
- StringToObjectBooleanTypeConverter
- StringToObjectByteTypeConverter
- StringToObjectDateTimeTypeConverter
- StringToObjectDecimalTypeConverter
- StringToObjectDoubleTypeConverter
- StringToObjectFloatTypeConverter
- StringToObjectIntTypeConverter
- StringToObjectLongTypeConverter
- StringToObjectShortTypeConverter
- StringToObjectStringTypeConverter

Notes
- If the default converter can't figure out how to convert a property, you will get an excpetion.  You have a couple of choose at that point:
    - Mark the property as "ignored when reading": [CsvConverter(IgnoreWhenReading = true)] 
    - Create a custom converter and add an attribute above the property: [CsvToClassTypeConverter(typeof(DecimalToIntCsvToClassConverter))]
