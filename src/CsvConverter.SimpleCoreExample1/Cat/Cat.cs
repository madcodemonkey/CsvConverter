using CsvConverter;

namespace SimpleCoreExample1
{
    public class Cat
    {
        [CsvConverterString(ColumnName = "Cat Name", ColumnIndex = 1)]
        public string Name { get; set; }

        [CsvConverterNumber(ColumnName = "Cat Age", ColumnIndex = 3)]
        public int Age { get; set; }

        [CsvConverter(typeof(CsvConverterCatType), ColumnIndex = 2)]
        public CatTypesEnum CatType { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}  Age: {Age}  Type: {CatType}";
        }
    }
}
