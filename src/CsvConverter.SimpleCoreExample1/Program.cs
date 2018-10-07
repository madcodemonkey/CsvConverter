using CsvConverter;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleCoreExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageDirectory = "c:\\temp";
            if (Directory.Exists(storageDirectory) == false)
                Directory.CreateDirectory(storageDirectory);

            string catFileName =  Path.Combine(storageDirectory, "Cats.csv");

            List<Cat> originalCatList = CreateCats(1);
            ShowCats("Generated file", originalCatList);

            WriteCats(originalCatList, catFileName);
            List<Cat> readCatList = ReadCats(catFileName);

            ShowCats("From file", readCatList);

            Console.WriteLine("Hit enter to exit");
            Console.ReadLine();
        }

        private static void ShowCats(string title, List<Cat> catList)
        {
            Console.WriteLine(title);
            foreach(var cat in catList)
            {
                Console.WriteLine(cat.ToString());
            }
        }

        private static List<Cat> ReadCats(string catFileName)
        {
            var result = new List<Cat>();

            using (var fs = File.OpenRead(catFileName))
            using (var sr = new StreamReader(fs))
            {
                var reader = new CsvReaderService<Cat>(sr);
                reader.Configuration.HasHeaderRow = true;
                reader.Configuration.BlankRowsAreReturnedAsNull = true;
                while (reader.CanRead())
                {
                    Cat item = reader.GetRecord();
                    if (item != null)
                       result.Add(item);
                }
            }

            return result;
        }

        private static void WriteCats(List<Cat> originalCatList, string catFileName)
        {
            using (var fs = File.Create(catFileName))
            using (var sw = new StreamWriter(fs))
            {
                fs.Seek(0, SeekOrigin.Begin);
                var writer = new CsvWriterService<Cat>(sw);
                writer.Configuration.HasHeaderRow = true;
                foreach(var cat in originalCatList)
                {
                    writer.WriterRecord(cat);
                }
            }

        }

        private static List<Cat> CreateCats(int numberOfCatsToCreate)
        {
            Random rand = new Random(DateTime.Now.Second);
            var result = new List<Cat>();
            for(int i =0; i < numberOfCatsToCreate; i++)
            {
                var oneCat = new Cat()
                {
                    Name = $"Cat Number {i}",
                    Age = rand.Next(1, 15),
                    CatType = (CatTypesEnum)rand.Next(1, 10)
                };
                result.Add(oneCat);
            }
            return result;
        }
    }
}
