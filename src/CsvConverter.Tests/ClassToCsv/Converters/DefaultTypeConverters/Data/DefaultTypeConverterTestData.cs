using System;

namespace CsvConverter.Tests.Shared
{
    internal class DefaultTypeConverterTestData
    {
        public String SomeStringProperty { get; set; }
        public DateTime SomeDateTimeProperty { get; set; }
        public DateTime? SomeNullableDateTimeProperty { get; set; }

        public bool SomeBoolProperty { get; set; }
        public bool? SomeNullableBoolProperty { get; set; }

        public byte SomeByteProperty { get; set; }
        public byte? SomeNullableByteProperty { get; set; }

        public int SomeIntProperty { get; set; }
        public int? SomeNullableIntProperty { get; set; }

        public long SomeLongProperty { get; set; }
        public long? SomeNullableLongProperty { get; set; }

        public short SomeShortProperty { get; set; }
        public short? SomeNullableShortProperty { get; set; }

        public decimal SomeDecimalProperty { get; set; }
        public decimal? SomeNullabeDecimalProperty { get; set; }

        public double SomeDoubleProperty { get; set; }
        public double? SomeNullableDoubleProperty { get; set; }

        public float SomeFloatProperty { get; set; }
        public float? SomeNullableFloatProperty { get; set; }

    }
}
