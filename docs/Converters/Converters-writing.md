# How do converters work when writing CSV files?

## Writing order
When writing a CSV file, the following sequence of events are followed:
1. Write a class instance (normally using the CsvWriterService class WriteRecord method) 
1. Iterate of the class instance propeties
1. For each property, call the converter that would convert the type into a string.
    - This could be a custom type converter created by you.
    - This could be one of the default converters if nothing was specified; however, if the their is no converter for the type, you will get an exception.
1. For each property, call ever converter marked as a post-conveter in the order specified by the Order property.  This will produce some final string.
1. All the string are placed in a List<string> and pass to a RowWriter
1. A row is written to the CSV file.


## Writing Rules
- Type convertering 
    - The converter used must inherit directly from CsvConverterTypeBase or a class that inherits from CsvConverterTypeBase (like CsvConverterStringBase).
    - There can only be one converter for converting the type.  
    - It's the one where IsPreConverter and IsPostConverter are both false.
- post-convertering
    - They are just string type converters that inherit from CsvConverterStringBase.
    - When writing, you may have as many as you wish.
    - They will be executed in the order specified by the Order property on the attribute.
    - They are executed AFTER the type converting is done so that your only dealing with strings.


