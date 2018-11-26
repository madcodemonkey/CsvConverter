# What are converters and how do I use them?
Converters are used to convert a string to a type (e.g., int, decimal, etc.) or to convert a type to a string; thus, converters are used for reading and writing CSV files.  Even if you do not explicitly specify a converter, a default converter will be used based on a class property type.  There are a number of built in converters, but you are free to create your own if these don't suit your needs.

## How do converters work?
- [How do converters work when reading CSV files?](./How-they-work/Converters-reading.md)
- [How do converters work when writing CSV files?](./How-they-work/Converters-writing.md)
- [How do you use a string converter as a pre-processor?](./How-they-work/Converters-preprocessing.md)
- [How do you use a string converter as a post-processor?](./How-they-work/Converters-postprocessing.md)

## What built in converters come with the project?
- [CsvConverterPercentage](./Built-In/Converters-CsvConverterPercentage.md)
- [CsvConverterStringReplaceTextEveryMatch](./Built-In/Converters-CsvConverterStringReplaceTextEveryMatch.md)
- [CsvConverterStringReplaceTextExactMatch](./Built-In/Converters-CsvConverterStringReplaceTextExactMatch.md)
- [CsvConverterStringTrimmer](./Built-In/Converters-CsvConverterStringTrimmer.md)
- [CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue](./Built-In/Converters-CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue.md)

## How do I create a custom converter?
- [How do you create a custom converter?](./Custom/Converters-custom.md)
- [How do you create a string converter?](./Custom/Converters-string.md)
