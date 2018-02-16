namespace CsvConverter.TypeConverters
{
    public class DoubleConverterSettings : IDoubleConverterSettings
    {
        public DoubleConverterSettings()  {  }
        public DoubleConverterSettings(int numberOfDecimalPlaces)
        {
            NumberOfDecimalPlaces = numberOfDecimalPlaces;
        }

        public int NumberOfDecimalPlaces { get; set; } = -1;
    }
}
