using CsvConverter;

namespace SimpleDotNetConsoleExample2.Model;

public class Squirrel
{
    // The CsvConverterStringTrimmer will only be run when reading a file. 
    [CsvConverterString(typeof(CsvConverterStringTrimmer), IsPreConverter = true)]
    public string Name { get; set; } = string.Empty;

    // The CsvConverterStringTrimmer will run when reading OR writing a file
    [CsvConverterString(typeof(CsvConverterStringTrimmer))]
    public string Species { get; set; } = string.Empty;

    // The CsvConverterStringTrimmer will run when writing a file
    [CsvConverterString(typeof(CsvConverterStringTrimmer), IsPostConverter = true)]
    public string HairColor { get; set; } = string.Empty;

    // The number 2 will be written as "two" (see the file)
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), NewValue = "2", OldValue = "two", IsPreConverter = true)]
    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextEveryMatch), NewValue = "two", OldValue = "2", IsPostConverter = true)]
    public int Age { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}  Age: {Age}  Hair Color: {HairColor} Species: {Species}";
    }
}