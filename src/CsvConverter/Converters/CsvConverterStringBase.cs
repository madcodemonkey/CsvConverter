namespace CsvConverter
{
    /// <summary>Base class for all string converters.</summary>
    public abstract class CsvConverterStringBase : CsvConverterTypeBase
    {
        public int Order { get; set; }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterBaseAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
        {
            base.Initialize(attribute, defaultFactory);

            if (!(attribute is CsvConverterStringAttribute oneAttribute))
                throw new CsvConverterAttributeException(
                    $"All string converters should be used with attributes that derive from the " +
                    $"{nameof(CsvConverterStringAttribute)}!");

            Order = oneAttribute.Order;
        }
    }
}
