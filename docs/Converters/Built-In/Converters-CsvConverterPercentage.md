# CsvConverterPercentage
This converter turns a string that is a percentage into a decimal value or throws an exception if the conversion fails OR converts a decimal into a percentage string

## Which attribute do I use?
It should be used with the special *CsvConverterNumber* attribute.

## Example 
```c#
public class Graduation
{
	[CsvConverterString(ColumnName = "First Name", ColumnIndex = 1)]
	public string FirstName { get; set; }
   
	  // You could delete NumberOfDecimalPlaces and specify AllowRounding = false and StringFormat = "P2" here as well and the output would be the same when writing to a CSV file.
      [CsvConverterNumber(ColumnName = "High School Graduation Percentile", ConverterType = typeof(CsvConverterPercentage), 
	    ColumnIndex = 2, NumberOfDecimalPlaces = 4)]
	public decimal? HighSchoolGraduationPercentile { get; set; }
}
```

Note
- See the GraduationDataTest.cs integration test in the CsvConverter.Core.IntegrationTests project for a more detailed example.