namespace CsvConverter
{
    /// <summary>Nain interface for STRING converters.</summary>
    public interface ICsvConverterString : ICsvConverter
    {
        /// <summary>The order of use when the converter is used as a pre or post converter and more than one is specified on a property</summary>
        int Order { get; set; }
    }
}
