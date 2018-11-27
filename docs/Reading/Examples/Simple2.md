# Reading CSV files: Simple Example 2

In this simple example, the CSV file does NOT have a header row.

**The main difference between this an other examples is that the service is TOLD that there is no header row via the Configuration property**

## Simple Example
Given this CSV file named c:\temp\Employee.csv
```
First1902,Last4658,Blue,4.5454545454545454545454545455,23
First181,Last2514,White,15.454545454545454545454545455,37
First2865,Last1905,Red,10.909090909090909090909090909,19
First4976,Last1577,Blue,12.727272727272727272727272727,53
First3401,Last381,Red,13.636363636363636363636363636,50
First4427,Last4967,Red,10.909090909090909090909090909,41
First4679,Last3868,Red,8.181818181818181818181818182,71
First4254,Last4574,Blue,6.3636363636363636363636363636,43
First3588,Last3581,Gray,10.909090909090909090909090909,31
First2926,Last106,Gray,11.818181818181818181818181818,54
```
and this class
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

You can read it in by doing the following:
```c#
using CsvConverter;
using System.IO;
 
using (var fs = File.OpenRead(dialog.FileName))
using (var sr = new StreamReader(fs, Encoding.Default))
{
    var csv = new CsvReaderService<Frog>(sr);
    // NO HEADER ROW
    // NO HEADER ROW
    // NO HEADER ROW
    csv.Configuration.HasHeaderRow = false;
    csv.Configuration.BlankRowsAreReturnedAsNull = true;

    while (csv.CanRead())
    {
        Frog record = csv.GetRecord();
        LogMessage(record.ToString());
    }
}
```

Notes
- The CsvReaderService service must be TOLD that the file has no header file (csv.Configuration.HasHeaderRow = false;)
- This example code can be found in [Github](../../../src/Examples/CsvConverter.SimpleDotNetExample2)


