namespace CsvConverter
{
    /// <summary>Nain interface for STRING converters.</summary>
    public interface ICsvConverterString : ICsvConverter
    {
        int Order { get; set; }
    }
}
