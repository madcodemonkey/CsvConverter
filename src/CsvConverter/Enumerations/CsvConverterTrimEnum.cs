namespace CsvConverter
{
    /// <summary>Tye type of trim actions that can be performed.</summary>
    public enum CsvConverterTrimEnum
    {
        /// <summary>Trim the start and the end</summary>
        All = 1,

        /// <summary>Trims just the start of the text</summary>
        TrimStart,

        /// <summary>Trims just the end of the text</summary>
        TrimEnd
    }
}
