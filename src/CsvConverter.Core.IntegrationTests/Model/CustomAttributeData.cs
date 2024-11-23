namespace CsvConverter.Core.IntegrationTests;

public class CustomAttributeData
{
    public int Id { get; set; }

    [CsvConverterNumber(typeof(CsvConverterDecimalToInt), AllowRounding = true, Mode = MidpointRounding.ToZero)]
    public int Speed { get; set; }

    [CsvConverterNumber(typeof(CsvConverterCommaDelimitedIntArray))]
    public int[] Items { get; set; } = Array.Empty<int>();

    [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue), NewValue = "Bob")]
    public string TextValue { get; set; } = string.Empty;
}