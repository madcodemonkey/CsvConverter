using System.Collections.Generic;

namespace SimpleExample1
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal PercentageBodyFat { get; set; }
        public double AvgHeartRate { get; set; }

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
