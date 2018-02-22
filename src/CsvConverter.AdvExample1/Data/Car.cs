using CsvConverter;

namespace AdvExample1
{
    public class Car
    {
        [TextLengthEnforcerConverter(MaximumLength = 8, MinimumLength = 5, CharacterToAddToShortStrings = '*')]
        public string Model { get; set; }

        [TextLengthEnforcerConverter(MaximumLength = 6, MinimumLength = 4, CharacterToAddToShortStrings = '~')]
        public string Make { get; set; }

        public int Year { get; set; }

        [MoneyFormatterConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format = "C2")]
        [CsvConverterCustom(typeof(MoneyTypeConverter))]
        public decimal PurchasePrice { get; set; }
        
        [MoneyFormatterConverter(typeof(MoneyFormatterClassToCsvTypeConverter), Format ="C2")]
        [CsvConverterCustom(typeof(MoneyTypeConverter))]
        public double CurrentValue { get; set; }

        public override string ToString()
        {
            return string.Format("Model: {0} Make: {1} Year: {2} PurchasePrice {3}  CurrentValue {4}",
                Model,
                Make,
                Year,
                PurchasePrice,
                CurrentValue);
        }
    }
}
