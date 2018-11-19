# Upgrading from V1 to V2

- The reader and writer service are now both in the CsvConverter namespace
    - Delete all references to the CsvConverter.CsvToClass namespace 
    - Delete all references to the CsvConverter.ClassToCsv namespace 
- Reader and writer services have been renamed.
    - Replace ClassToCsvService with CsvWriterService
    - Replace CsvToClassService with CsvReaderService
- Configuration settings
    - IgnoreBlankRows property on the reaader or writer service,  Configuration.IgnoreBlankRows, was renamed to BlankRowsAreReturnedAsNull because that's what it was really doing in V1.
    -
