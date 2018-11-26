# Reading CSV files

To read a CSV file into a class, there are 8 basic steps
1. Add the CsvConverter NuGet package to your project
2. Create a class
3. Optional: Attribute the class with the [CsvConverter] attribute if a column name is different from a class property name.
4. Instantiate the CsvReaderService class
5. Optional: Set configuration settings on the CsvReaderService instance (see Configuration property)
6. Add CsvConverter namespace to your using statements.
7. In a while loop as long a CanRead() is true, you can read records
8. Call GetRecord() for each row in the CSV file. 

## Examples
- [Simple Example 1](./Examples/Simple1.md) - In this simple example, the CSV file has a header row and the column names match the class property names perfectly.
- [Simple Example 2](./Examples/Simple2.md) - In this simple example, the CSV file does NOT have a header row.
- [Simple Example 3](./Examples/Simple3.md) - In this simple example, the CSV file has a header row and the column names DO NOT match the class property names perfectly.