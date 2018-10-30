using CsvConverter;

namespace AdvExample3
{
    public class Car
    {
        [CsvConverterTextLengthEnforcer(MaximumLength = 8, MinimumLength = 5, 
            CharacterToAddToShortStrings = '*', IsPreConverter = true)]
        public string Model { get; set; }

        [CsvConverterTextLengthEnforcer(MaximumLength = 6, MinimumLength = 4, 
            CharacterToAddToShortStrings = '~', IsPreConverter = true)]
        public string Make { get; set; }

        public int Year { get; set; }

        [CsvConverterNumber(ConverterType = typeof(CsvConverterMoney), StringFormat = "C2")]
        public decimal PurchasePrice { get; set; }
        
        [CsvConverterNumber(ConverterType = typeof(CsvConverterMoney), StringFormat ="C2")]
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
