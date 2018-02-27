 # Writing CSV files: Default converters

 All the default type converters are held within the ClassToCsvService class DefaultConverters property, which helps it convert an object into a string.  These are the default/fallback type converters that are used when no custom type converter is specified via an attribute.  These type converters convert class properties into strings.  The default set of converters only handle privitives.  Once a default type converter converts an object into a string, it is written out to the CSV file.  The DefaultConverters property is called in the CreateDataRow() method, which is called by the public WriterRecord method, if a custom type converter is not specified and is passed to the custom type converter if one is specified.

The DefaultConverters property will return an interface of IDefaultObjectToStringTypeConverterManager, which is defined as follows:
```C#
 public interface IDefaultObjectToStringTypeConverterManager
{ 
    string Convert(Type theType, object value, string stringFormat, string columnName, int columnIndex, int rowNumber);
    void AddConverter(Type theType, IClassToCsvTypeConverter converter);
    bool ConverterExists(Type theType);
    T FindConverter<T>(Type theType) where T : class;
    void RemoveConverter(Type theType);
    void UpdateBooleanSettings(IBooleanConverterSettings settings);
}
```

Notes
- You can add and remove converters using AddConverter and RemoveConverter.
- These converters are type specific and will be called for a given type when a custom converter is NOT specified.
- You could add a type converter for non-privitive types (Class properties, arrays, etc.). For example, if you want it to handle typeof(int[]), you could add a new converter to handle it. 
- There are some settings for booleans that you can update as well which specify how you would like it written (T, F, True, False, Yes, No, etc.).

## Default type converters
If you do NOT specify a type converter for a property, one of these default converters will used:
- ObjectToStringBooleanTypeConverter
- ObjectToStringByteTypeConverter
- ObjectToStringDateTypeConverter
- ObjectToStringDecimalTypeConverter
- ObjectToStringDoubleTypeConverter
- ObjectToStringFloatTypeConverter
- ObjectToStringIntTypeConverter
- ObjectToStringLongTypeConverter
- ObjectToStringShortTypeConverter

Notes
- If the default type converter can't figure out how to convert a property, you will get an excpetion.  You have a couple of options at that point:
    - Mark the property as "ignored when writing": [CsvConverter(IgnoreWhenWriting = true)] 
    - Create a custom converter and add an attribute above the property:  [CsvConverterCustom(typeof(TrimClassToCsvTypeConverter))]
