using CsvConverter;

namespace SimpleDotNetExample2
{
    public class Frog
    {
        [CsvConverter(ColumnIndex = 1)]
        public string FirstName { get; set; }

        [CsvConverter(ColumnIndex = 2)]
        public string LastName { get; set; }

        [CsvConverter(ColumnIndex = 5)]
        public int Age { get; set; }

        [CsvConverter(ColumnIndex = 4)]
        public decimal AverageNumberOfSpots { get; set; }

        [CsvConverter(ColumnIndex = 3)]
        public string Color { get; set; }

        public override string ToString()
        {
            return string.Format("FirstName: {0} LastName: {1} Age: {2} AverageNumberOfSpots: {3} Color: {4}",
                FirstName,
                LastName,
                Age,
                AverageNumberOfSpots,
                Color);
        }
    }
}
