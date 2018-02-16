# Reading CSV files

To read a CSV file into a class, there are 7 basic steps
1. Add the CsvConverter NuGet package to your project
2. Create a class
3. Optional: Attribute the class with the [CsvConverter] attribute if the column name is different from the class property name.
4. Instantiate the CsvToClassService class
5. Optional: Set configration settings on the CsvToClassService instance (see Configuration property)
6. In a while loop as long a CanRead() is true, you can read records
7. Call GetRecord() for each row in the CSV file. 

## Examples
- [Simple Example 1](./Examples/Simple1.md) - In this simple example, the CSV file has a header row and the column names match the class properties perfectly.
- [Simple Example 2](./Examples/Simple2.md) - In this simple example, the CSV file has a header row and the column names DO NOT match the class properties perfectly.


## Advanced Topics
- [Converters](./Converters/Main.md)
    - [Built in converters](./Converters/Built-in.md)
    - [Creating your own converters](./Converters/Creating.md)
- [Pre-processors](./Preprocesors/Preprocesors.md)
    - [Built in pre-processors](./Preprocesors/Built-in.md)
    - [Creating your own pre-processors](./Preprocesors/Creating.md)
