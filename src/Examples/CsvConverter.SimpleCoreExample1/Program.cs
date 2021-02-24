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
            var items = CreateItems();

            string writeFile = @"C:\Temp\Book2.csv";
            WriteItems(items, writeFile);

            string readFile = @"C:\Temp\Book2.csv";
            ReadItems(readFile);

            Console.WriteLine("done");
            Console.ReadLine();
        }

        private static List<TestData> CreateItems()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            var data = new List<TestData>();
            for (int i = 0; i < 20; i++)
            {
                data.Add(new TestData()
                {
                    FieldName = $"Field{i}",
                    FieldDescription = $"This is field number {i}",
                    EndPosition = $"some end data {rand.Next(1, 200000)}",
                    StartPosition = $"{rand.Next(1, 5000000)} some start data "
                });
            }

            return data;
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

        private static void ReadItems(string fileName)
        {
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
                        Console.WriteLine(item.FieldDescription);
                    }
                }
            }
        }
    }
}
