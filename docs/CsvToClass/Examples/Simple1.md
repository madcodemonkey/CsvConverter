# Simple Example 1

In this simple example, the CSV file has a header row and the column names match the class properties perfectly.

## Simple Example
Give this CSV file named c:\temp\people.csv
```
FirstName,LastName,Age,PercentageBodyFat
John,Smith,45,7.54
Jane,Smith,41,11.34
Thomas,Adams,12,5.54
```
and this class
```c#
public class Person
{
	public string FirstName { get; set; }
	
    public string LastName { get; set; }
	
    public int Age { get; set; }

	public double? PercentageBodyFat { get; set; }
}
```

You can read it in by doing the following:
```c#
using CsvConverter;
using System.IO;

using (var fs = File.OpenRead("c:\\temp\\people.csv"))
using (var sr = new StreamReader(fs, Encoding.Default))
{                
	var csv = new CsvToClassService<Person>(sr);
	csv.Configuration.IgnoreBlankRows = true;

	while (csv.CanRead())
	{
		Person record = csv.GetRecord();
	}
}
```
