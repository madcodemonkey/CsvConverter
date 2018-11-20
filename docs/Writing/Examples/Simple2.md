 # Writing CSV files: Simple Example 2

In this simple example, we will control the order and names of the CSV headers.

## Simple Example
Given this class
```c#
public class Frog
{
    [CsvConverter(ColumnIndex = 1)]
    public string FirstName { get; set; }

    [CsvConverter(ColumnIndex = 2)]
    public string LastName { get; set; }

    [CsvConverter(ColumnIndex = 5)]
    public int Age { get; set; }

    [CsvConverter(ColumnIndex = 4)]
    public decimal AverageNumberOfSpots { get; set; }

    [CsvConverter(ColumnIndex = 3)]
    public string Color { get; set; }

    public override string ToString()
    {
        return string.Format("FirstName: {0} LastName: {1} Age: {2} AverageNumberOfSpots: {3} Color: {4}",
            FirstName,
            LastName,
            Age,
            AverageNumberOfSpots,
            Color);
    }
}
```

We can write this code:
```c#
using System.IO;
using System.Text;
using CsvConverter.ClassToCsv;

Random rand = new Random(DateTime.Now.Second);
int numberToCreate = rand.Next(10, 100);

using (var fs = File.Create(dialog.FileName))
using (var sw = new StreamWriter(fs, Encoding.Default))
{
	var writerService = new CsvWriterService<Frog>(sw);
	writerService.Configuration.HasHeaderRow = false;

	for (int i = 0; i < numberToCreate; i++)
	{
		var newEmp = new Frog()
		{
			FirstName = $"First{rand.Next(1, 5000)}",
			LastName = $"Last{rand.Next(1, 5000)}",
			Age = rand.Next(5, 80),
			Color = PickRandomColor(rand.Next(1, 5)),
			AverageNumberOfSpots = rand.Next(5, 20) / 1.1m
		};

		writerService.WriterRecord(newEmp);
	}
}
```

To produce this CSV file:
```
FirstName,LastName,Color,AverageNumberOfSpots,Age
First1363,Last729,Red,14.545454545454545454545454545,24
First3816,Last2396,Blue,10.909090909090909090909090909,57
First3215,Last3783,Blue,14.545454545454545454545454545,73
First552,Last3177,Gray,14.545454545454545454545454545,25
First1228,Last3550,Gray,13.636363636363636363636363636,58
First4244,Last1595,Gray,8.181818181818181818181818182,51
First1242,Last720,Blue,13.636363636363636363636363636,45
First1271,Last1870,Blue,9.090909090909090909090909091,13
First4140,Last3531,White,6.3636363636363636363636363636,62
```

Notes
- The columns are sorted by Column Index (the normal sort order is alphabetical if not specified with ONE BASED ColumnIndex number).
- If you had columns without the ColumnIndex, they would be sorted alphabetical because the sort order is ColumnIndex and then ColumnName.
- This example code can be found in [Github](https://github.com/madcodemonkey/CsvConverter/tree/master/src/CsvConverter.SimpleExample1)
