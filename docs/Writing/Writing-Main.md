# Writing CSV files

To create a CSV file, there are 7 basic steps
1. Add the CsvConverter NuGet package to your project
2. Create a class
3. Optional: Attribute the class with the [CsvConverter] attribute if the column names should be different from the class property names.  You can also control the order the columns are written by using the ONE based ColumnIndex as well.
4. Instantiate the CsvWriterService class
5. Optional: Set configuration settings on the CsvWriterService instance (see Configuration property)
6. Add CsvConverter namespace to your using statements.
7. Call WriteRecord() for each row you want to write in the CSV file. 

## Examples
- [Simple Example 1](./Examples/Simple1.md) - In this simple example, the header will match the property names.
- [Simple Example 2](./Examples/Simple2.md) - In this simple example, we will control the order of the CSV columns.
