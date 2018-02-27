# Reading CSV files

To read a CSV file into a class, there are 8 basic steps
1. Add the CsvConverter NuGet package to your project
2. Create a class
3. Optional: Attribute the class with the [CsvConverter] attribute if the column name is different from the class property name.
4. Instantiate the CsvToClassService class
5. Optional: Set configration settings on the CsvToClassService instance (see Configuration property)
6. Add CsvConverter.CsvToClass namespace to your using statements.
7. In a while loop as long a CanRead() is true, you can read records
8. Call GetRecord() for each row in the CSV file. 

## Examples
- [Simple Example 1](./Examples/Simple1.md) - In this simple example, the CSV file has a header row and the column names match the class properties perfectly.
- [Simple Example 2](./Examples/Simple2.md) - In this simple example, the CSV file does NOT have a header row.
- [Simple Example 3](./Examples/Simple3.md) - In this simple example, the CSV file has a header row and the column names DO NOT match the class properties perfectly.


## Advanced Topics
-  [Pre Converters](./PreConverters/PreConverters-Main.md)  - Do some pre-processing on the CSV column string BEFORE passing it to a TYPE converter for further processing.
-  [TYPE Converters](./TypeConverters/TypeConverters-Main.md) - Converts strings into C# types.  The string can come directly from the CSV file OR it could have passed through one or more pre-converters.
