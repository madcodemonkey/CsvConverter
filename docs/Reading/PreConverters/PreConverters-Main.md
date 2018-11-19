# Reading CSV files:  Pre-processors

When reading, you may have as many [pre-converters](./PreConverters/PreConverters-Main.md) as you wish, but only one [type converters](../TypeConverters/TypeConverters-Main.md).  [Pre-converters](./PreConverters/PreConverters-Main.md) are designed to manipulate the string before the [type converters](./TypeConverters/TypeConverters-Main.md)  converts it to the type required by the class property.  Common pre-converter operations may include trimming, removing characters and other similar string manipulation activities.  When creating [pre-converters](./PreConverters/PreConverters-Main.md), you should no longer think of them as a seperate type.  They now just STRING type converters.  Any string type converter, which inherits from CsvConverterStringBase, can be a pre-converter, type converter or post converter.


 Pre-converters topics
- [Custom pre-converters included with this library](./PreConverters-Included-Custom.md)
- [Creating your own pre-converters](./PreConverters-Creating-Custom.md)

