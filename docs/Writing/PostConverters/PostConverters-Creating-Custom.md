 # Writing CSV files: Creating your own post-converters

Custom post-converters can be created to process the CSV field string AFTER it has been converted to a string by a TYPE converter but BEFORE it is written to the CSV file.  This can be handy if you need to erase a value or augment the string is some way.

Examples
- [Example1](./PostConverters-Creating-Custom-Example1.md) - post-converter with a custom attribute.