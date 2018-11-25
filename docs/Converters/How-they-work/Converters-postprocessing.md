# How do you use a string converter as a post-processor?

A post-processor is JUST a string converter.  Any converter that inherits from CsvConverterStringBase is considered a string converter and can be used as a pre-processor, type converter or post-processor.

## Rules for pre-processors
- They are just string type converters that inherit from CsvConverterStringBase (Note: CsvConverterStringBase inherits from CsvConverterTypeBase).
- They must be used with the CsvConverterStringAttribute attribute OR an attribute that inherits from CsvConverterStringAttribute (e.g., CsvConverterStringTrimAttribute).  The reason for this requirement is that the CsvConverterStringAttribute has a IsPostConverter property that must be set to TRUE to indicate that you want a post-processor. 
- When writing, you may have as many as you wish.
- They will be executed in the order specified by the Order property on the attribute.  If you are only using one, don't bother specifying the Order.  However, you should always specify order if you are using more than one post-converters on a property.
- They are executed AFTER the type converting is done so that your only dealing with strings.

## Example 1
```c#

```

Notes

## Example 2
```c#

```

Notes
