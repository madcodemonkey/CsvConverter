# Upgrading from V1 to V2

- The reader and writer service are now both in the CsvConverter namespace
    - Delete all references to the CsvConverter.CsvToClass namespace 
    - Delete all references to the CsvConverter.ClassToCsv namespace 
- Reader and writer services have been renamed.
    - Replace ClassToCsvService with CsvWriterService
    - Replace CsvToClassService with CsvReaderService
- The writer services writing method was renamed from WriterRecord to WriteRecord so that it is truly the opposite of GetRecord, which is used when reading. Sadly this typo has been in the code for a while.
- Configuration settings
    - Configuration.IgnoreBlankRows property on the reader or writer service was renamed to BlankRowsAreReturnedAsNull because that's what it was really doing in V1.
- Built in pre-converters.  Any string converter can now be used as a pre-converter, so here are replacements for the old pre-converters:
    - TextReplacerCsvToClassPreConverter can be replaced with either of these:
        - CsvConverterStringReplaceTextEveryMatch
        - CsvConverterStringReplaceTextExactMatch
    - TrimCsvToClassPreConverter is replaced with CsvConverterStringTrimmer.
    - StringIsNullOrWhiteSpaceSetToNullCsvToClassPreConverter is replaced with CsvConverterStringReplaceNullOrWhiteSpaceWithNewValue
