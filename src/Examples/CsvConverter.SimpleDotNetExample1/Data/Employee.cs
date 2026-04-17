using System;

namespace SimpleDotNetExample1
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal PercentageBodyFat { get; set; }
        public double AvgHeartRate { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} FirstName: {FirstName} LastName: {LastName} Age: {Age} PercentageBodyFat: {PercentageBodyFat} AvgHeartRate: {AvgHeartRate}";
        }
    }
}
