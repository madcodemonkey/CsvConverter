# Reading CSV files:  Simple Example 1

In this simple example, the CSV file has a header row and the column names match the class properties perfectly.

## Simple Example
Given this CSV file named c:\temp\Employee.csv
```
Age,AvgHeartRate,FirstName,LastName,PercentageBodyFat
16,56.3636363636364,First645,Last42,14.166666666666666666666666667
25,68.1818181818182,First1707,Last208,0.8333333333333333333333333333
17,70,First2647,Last962,5
25,66.3636363636364,First3318,Last2876,3.3333333333333333333333333333
53,62.7272727272727,First1615,Last4816,8.333333333333333333333333333
```
and this class
```c#
public class Employee
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int Age { get; set; }
	public decimal PercentageBodyFat { get; set; }
	public double AvgHeartRate { get; set; }

	public override string ToString()
	{
		return string.Format("FirstName: {0} LastName: {1} Age: {2} PercentageBodyFat: {3} AvgHeartRate: {4}",
			FirstName,
			LastName,
			Age,
			PercentageBodyFat,
			AvgHeartRate);
	}
}
```

You can read it in by doing the following:
```c#
using CsvConverter.CsvToClass;
using System.IO;

using (var fs = File.OpenRead(dialog.FileName))
using (var sr = new StreamReader(fs, Encoding.Default))
{
	var csv = new CsvToClassService<Employee>(sr);
	csv.Configuration.IgnoreBlankRows = true;

	while (csv.CanRead())
	{
		Employee record = csv.GetRecord();
		LogMessage(record.ToString());
	}
}
```


Notes
- This example code can be found in [Github](../../../src/CsvConverter.SimpleExample1)

