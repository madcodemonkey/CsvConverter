using CsvConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleCoreExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            string readFile = @"C:\Temp\Book2.csv";
            var items = ReadItems(readFile);

            string writeFile = @"C:\Temp\Book2a.csv";
            WriteItems(items, writeFile);

            Console.WriteLine("done");
            Console.ReadLine();
        }

        private static void WriteItems(List<TestData> items, string fileName)
        {

            using (var fs = File.Create(fileName))
            using (var sw = new StreamWriter(fs, Encoding.Default))
            {
                var writerService = new CsvWriterService<TestData>(sw);
                foreach(var item in items)
                {
                     

                    writerService.WriteRecord(item);
                }
            }
        }

        private static List<TestData> ReadItems(string fileName)
        {
            var items = new List<TestData>();


            using (var fs = File.OpenRead(fileName))
            using (var sr = new StreamReader(fs))
            {
                ICsvReaderService<TestData> reader = new CsvReaderService<TestData>(sr);
                reader.Configuration.HasHeaderRow = true;
                reader.Configuration.BlankRowsAreReturnedAsNull = true;
                while (reader.CanRead())
                {
                    TestData item = reader.GetRecord();
                    if (item != null)
                    {
                        items.Add(item);
                        Console.WriteLine(item.FieldDescription);
                    }
                }
            }

            return items;
        }
    }
}
