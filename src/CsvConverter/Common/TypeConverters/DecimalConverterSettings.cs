namespace CsvConverter.TypeConverters
{
    public class DecimalConverterSettings : IDecimalConverterSettings
    {
        public DecimalConverterSettings() { }
        public DecimalConverterSettings(int numberOfDecimalPlaces)
        {
            NumberOfDecimalPlaces = numberOfDecimalPlaces;
        }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }
}
