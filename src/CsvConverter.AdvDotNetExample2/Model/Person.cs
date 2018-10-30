using CsvConverter;

namespace AdvExample2
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [CsvConverterNumericRange(typeof(CsvConverterNumericRange), Minimum = 1, Maximum = 50)]
        public int Age { get; set; }

        public decimal PercentageBodyFat { get; set; }
        public double AvgHeartRate { get; set; }

        // This is not necessary (objects are ignored unless the are decorated with a converter
        // [CsvConverter(IgnoreWhenReading = true, IgnoreWhenWriting = true)]
        public Person Parent { get; set; }

        public override string ToString()
        {
            return string.Format("FirstName: {0} LastName: {1} Age: {2} PercentageBodyFat: {3} AvgHeartRate: {4}",
                FirstName,
                LastName,
                Age,
                PercentageBodyFat,
                AvgHeartRate);
        }
    }
}
