# Reading CSV files: Converters 

Converts strings into C# types.  The string can come directly from the CSV file OR it could have passed through one or more [pre-converters](../PreConverters/PreConverters-Main.md).  There are two types of converters, default and custom type converters.  The default converters, which convert strings into types, and custom converters, which are created by you to convert strings into other types.

Rules for type converters
1. A property can have ONE and ONLY ONE type converter.  Attempting to assign a property more than one type converter on a property will result in an exception.
2. If you decorate the class with a type converter and specify the TargetPropertyType, it will NOT override a type converter on a property.  It is assumed that the one on the property is more specific and should be kept.

Converter topics
- [Default converters](./TypeConverters-Default.md)
- [Custom type converters included with this library](./TypeConverters-Included-Custom.md)
- [Creating your own custom type converter](./TypeConverters-Creating-Custom.md)