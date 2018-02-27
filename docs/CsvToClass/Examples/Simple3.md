# Reading CSV files:  Simple Example 3

In this simple example, the CSV file has a header row and the column names DO NOT match the class properties perfectly.

## Simple Example
Given this CSV file named c:\temp\person.csv
```
FName,LName,ADAT34_R,PBF
John,Smith,45,7.54
Jane,Smith,41,11.34
Thomas,Adams,12,5.54
```
and this class
```c#
public class Person
{
	[CsvConverter(ColumnName ="FName")]
	public string FirstName { get; set; }

	[CsvConverter(ColumnName ="LName")]
	public string LastName { get; set; }

	[CsvConverter(ColumnName ="ADAT34_R")]
	public int Age { get; set; }

	[CsvConverter(ColumnName ="PBF")]
	public double? PercentageBodyFat { get; set; }
}
```

You can read it in by doing the following:
```c#
using CsvConverter.CsvToClass;
using System.IO;

using (var fs = File.OpenRead("c:\\temp\\person.csv"))
using (var sr = new StreamReader(fs, Encoding.Default))
{                
	var csv = new CsvToClassService<Person>(sr);
	csv.Configuration.IgnoreBlankRows = true;

	while (csv.CanRead())
	{
		Person record = csv.GetRecord();
		// Do something with the instance of person
	}
}
```
