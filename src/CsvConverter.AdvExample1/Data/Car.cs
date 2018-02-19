namespace AdvExample1
{
    public class Car
    {
        [TextLengthEnforcerPreprocessor(MaximumLength = 8, MinimumLength = 5, CharacterToAddToShortStrings = '*')]
        public string Model { get; set; }

        [TextLengthEnforcerPreprocessor(MaximumLength = 6, MinimumLength = 4, CharacterToAddToShortStrings = '~')]
        public string Make { get; set; }

        public int Year { get; set; }

        public override string ToString()
        {
            return string.Format("Model: {0} Make: {1} Year: {2}",
                Model,
                Make,
                Year);
        }
    }
}
