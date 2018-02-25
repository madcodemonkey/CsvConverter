# Reading CSV files:  Creating your own pre-converters

Custom pre-converters can be created to process the CSV field string BEFORE they reach a TYPE converter.  This can be handy if you know there is something in the field that may not convert directly to a type.  For example, removing single quotes from around an integer so that it can be converted to a integer without error.  You may also want to create a custom attribute if you need special inputs for your pre-converter. 

Examples
- [Example1](./PreConverters-Creating-Custom-Example1.md) - pre-converter with a custom attribute.