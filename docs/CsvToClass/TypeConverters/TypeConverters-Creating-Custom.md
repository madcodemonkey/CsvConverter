# Reading CSV files:  Creating your own custom type converter

Custom type converters can be created to handle different types **or**  to make a tweeks to strings before passing them to the default type converter.  If your custom type converter needs some special inputs (e.g., two integers) you may want to start out by creating a new attribute and then creating a converter to pass to it.

Examples
- [Example 1](./TypeConverters-Creating-Custom-Example1.md) - Type converter only with NO custom attribute.
- [Example 2](./TypeConverters-Creating-Custom-Example2.md) - Type converter with a custom attribute.