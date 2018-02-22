namespace CsvConverter.TypeConverters
{
    public class DecimalPlacesSettings : IDecimalPlacesSettings
    {
        public DecimalPlacesSettings() { }
        public DecimalPlacesSettings(int numberOfDecimalPlaces)
        {
            NumberOfDecimalPlaces = numberOfDecimalPlaces;
        }
        public int NumberOfDecimalPlaces { get; set; } = -1;
    }
}
