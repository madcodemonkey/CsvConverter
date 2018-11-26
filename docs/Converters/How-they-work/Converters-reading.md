# How do converters work when reading CSV files?

## Reading order
When reading a CSV file, the following sequence of events are followed:
1. Read a line (normally using the CsvReaderService class GetRecord method) 
1. Parse the line into columns
1. Iterate over the columns
1. For each column, call ever converter marked as a pre-converter in the order specified by the Order property.  This will produce some final string.
1. For each column, call the converter that would convert the string into a type.
    - This could be a custom type converter created by you.
    - This could be one of the default converters if nothing was specified; however, if the there is no converter for the type, you will get an exception.

## Reading Rules
- pre-convertering
    - They are just string type converters that inherit from CsvConverterStringBase (Note: CsvConverterStringBase inherits from CsvConverterTypeBase).
    - They must be used with the CsvConverterStringAttribute attribute OR an attribute that herits from CsvConverterStringAttribute (e.g., CsvConverterStringTrimAttribute).  The reason for this requirement is that the CsvConverterStringAttribute has a IsPreConverter property that must be set to TRUE to indicate that you want a pre-converter.
    - When reading, you may have as many as you wish.
    - They will be executed in the order specified by the Order property on the attribute.
    - They are executed BEFORE type conversion takes place so that they are dealing with strings coming directly from the CSV file OR from the pre-converter that came before it.
- Type convertering 
    - All converters must inherit from CsvConverterTypeBase or a class that inherits from CsvConverterTypeBase (e.g., CsvConverterStringBase).
    - There can only be one converter for converting the type.  
    - The type converter is the one where IsPreConverter and IsPostConverter are both false.
    - Specifying more than one type converter results in an exception.
