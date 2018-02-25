# Reading CSV files: Default type converters

 All the default type converters are held within the CsvToClassService class DefaultConverters property, which helps it convert a string into an object.  These are the default/fallback type converters that are used when no custom type converter is specified via an attribute.  These default type converters convert basic strings into objects.  The default set of type converters only handle privitives, so arrays and classes are ignored by default.  Once the default type converter converts a string to an object, it assigns that object to a class property.  The DefaultConverters property is called in the GetRecord() method if a custom type converter is not specified and is passed to the custom converter if one is specified.

The DefaultConverters property will return an interface of IDefaultStringToObjectTypeConverterManager, which is defined as follows:
```C#
public interface IDefaultStringToObjectTypeConverterManager
{
	void AddConverter(Type outputType, ICsvToClassTypeConverter converter);
	object Convert(Type theType, string stringValue, string columnName, int columnIndex, int rowNumber);
	ICsvToClassTypeConverter FindConverter(Type theType);
	T FindConverter<T>(Type theType) where T : class;
	bool ConverterExists(Type typeToConvert);
	void RemoveConverter(Type typeToConvert);
	void UpdateDoubleSettings(IDecimalPlacesSettings settings);
	void UpdateDecimalSettings(IDecimalPlacesSettings settings);
	void UpdateDateTimeParsing(IDateConverterSettings settings);
}
```

Notes
- You can add and remove type converters using AddConverter and RemoveConverter.
- These converters are type specific and will be called for a given type when a custom converter is NOT specified.
- You could add a type converter for non-privitive types (Class properties, arrays, etc.). For example, if you want it to handle typeof(int[]), you could add a new converter to handle it. 
- There are some settings for doubles, decimals, and dates that you can update as well.

## Default type converters
If you do NOT specify a type converter for a property, one of these default type converters will used:
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
- If the default type converter can't figure out how to convert a property, you will get an excpetion.  You have a couple of choose at that point:
    - Mark the property as "ignored when reading": [CsvConverter(IgnoreWhenReading = true)] 
    - Create a custom type converter and add an attribute above the property: [CsvConverterCustom(typeof(DecimalToIntCsvToClassConverter))]