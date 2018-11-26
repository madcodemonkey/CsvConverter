# How do converters work when writing CSV files?

## Writing order
When writing a CSV file, the following sequence of events are followed:
1. Write a class instance (normally using the CsvWriterService class WriteRecord method) 
1. Iterate over the class instance propeties
1. For each property, call the converter that would convert the type into a string.
    - This could be a custom type converter created by you.
    - This could be one of the default converters if nothing was specified; however, if the their is no converter for the type, you will get an exception.
1. For each property, call ever converter marked as a post-conveter in the order specified by the Order property.  This will produce some final string.
1. All the strings are placed in a List<string> and passed to an instance of the RowWriter class.
1. A row is written to the CSV file.


## Writing Rules
- Type convertering 
    - All converters used must inherit from CsvConverterTypeBase or a class that inherits from CsvConverterTypeBase (e.g., CsvConverterStringBase).
    - There can only be one converter for converting the type.  
    - It's the one where IsPreConverter and IsPostConverter are both false.
    - Specifying more than one type converter results in an exception.
- post-convertering
    - They are just string type converters that inherit from CsvConverterStringBase (Note: CsvConverterStringBase inherits from CsvConverterTypeBase).
    - They must be used with the CsvConverterStringAttribute attribute OR an attribute that inherits from CsvConverterStringAttribute (e.g., CsvConverterStringTrimAttribute).  The reason for this requirement is that the CsvConverterStringAttribute has a IsPostConverter property that must be set to TRUE to indicate that you want a post-processor. 
    - When writing, you may have as many as you wish.
    - They will be executed in the order specified by the Order property on the attribute.  If you are only using one, don't bother specifying the Order.  However, you should always specify order if you are using more than one post-converters on a property.
    - They are executed AFTER the type converting is done so that your only dealing with strings.


