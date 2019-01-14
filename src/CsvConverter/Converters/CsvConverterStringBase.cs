namespace CsvConverter
{
    /// <summary>Base class for all string converters.</summary>
    public abstract class CsvConverterStringBase : CsvConverterTypeBase
    {
        /// <summary>The order of use when the converter is used as a pre or post converter and more than one is specified on a property</summary>
        public int Order { get; set; }

        /// <summary>Initializes the converter with an attribute</summary>
        public override void Initialize(CsvConverterAttribute attribute, IDefaultTypeConverterFactory defaultFactory)
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
