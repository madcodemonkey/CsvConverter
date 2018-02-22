namespace CsvConverter
{

    public interface ICsvConverter
    {
        /// <summary>Converter type</summary>
        CsvConverterTypeEnum ConverterType { get; }

        /// <summary>The order that the converter should be processed if there is more than one.</summary>
        int Order { get; }

        /// <summary>Used to pass the attribute to the converter in case it needs any optional inputs.</summary>
        void Initialize(CsvConverterCustomAttribute attribute);
    }
}
