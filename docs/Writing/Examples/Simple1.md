# Writing CSV files: Simple Example 1

In this simple example, the headers will match the property names.

## Simple Example
Given this class
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

We can write this code:
```c#
using System.IO;
using System.Text;
using CsvConverter;

Random rand = new Random(DateTime.Now.Second);
int numberToCreate = rand.Next(10, 100);

using (var fs = File.Create(dialog.FileName))
using (var sw = new StreamWriter(fs, Encoding.Default))
{
	var writerService = new CsvWriterService<Employee>(sw);
	for (int i = 0; i < numberToCreate; i++)
	{
		var newEmp = new Employee()
		{
			FirstName = $"First{rand.Next(1,5000)}",
			LastName = $"Last{rand.Next(1, 5000)}",
			Age = rand.Next(5, 80),
			PercentageBodyFat = rand.Next(1, 20) / 1.2m,
			AvgHeartRate = rand.Next(60, 80) / 1.1
		};

		writerService.WriterRecord(newEmp);
	}
}
```

To produce this CSV file:
```
Age,AvgHeartRate,FirstName,LastName,PercentageBodyFat
16,56.3636363636364,First645,Last42,14.166666666666666666666666667
25,68.1818181818182,First1707,Last208,0.8333333333333333333333333333
17,70,First2647,Last962,5
25,66.3636363636364,First3318,Last2876,3.3333333333333333333333333333
53,62.7272727272727,First1615,Last4816,8.333333333333333333333333333
```

Notes
- The columns are sorted in alphabetical order by default
- To control how columns are output using the [CsvConveter(ColumnIndex=1)] attribute.  The column indexes are ONE based.
- This example code can be found in [Github](https://github.com/madcodemonkey/CsvConverter/tree/master/src/CsvConverter.SimpleExample1)

