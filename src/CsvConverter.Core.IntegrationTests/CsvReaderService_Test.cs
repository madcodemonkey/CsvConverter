using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.IntegrationTests
{
    // TODO: Need a mismatched reader test where there are 3 columns and a few rows only have 2 columns. Currently this would throw an exception if ThrowExceptionIfColumnCountChanges in configuration was false
    [TestClass]
    public class CsvReaderService_Test
    {
        [TestMethod]
        public void GetRecord_BlankRowsAreReturnedAsNull_BlankRowsIgnored()
        {
            var tempFileName = Path.GetTempFileName();
            try
            {
                using (var fs = File.Create(tempFileName))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine("Name,Age");
                    sw.WriteLine("Fido,1");
                    sw.WriteLine(",");
                    sw.WriteLine("");
                    sw.WriteLine("Killer,2");
                    sw.WriteLine("Jaws,3");
                    sw.WriteLine("   ");
                    sw.WriteLine("Fluffy,4");
                    sw.WriteLine("John the dog,5");
                    sw.WriteLine("   ");
                }

                var result = new List<Dog>();
                using (var fs = File.OpenRead(tempFileName))
                using (var sr = new StreamReader(fs))
                {
                    var reader = new CsvReaderService<Dog>(sr);
                    reader.Configuration.HasHeaderRow = true;
                    reader.Configuration.BlankRowsAreReturnedAsNull = true; // HERE IS WHAT WE ARE TESTING!!
                    while (reader.CanRead())
                    {
                        var data = reader.GetRecord();
                        if (data != null)
                           result.Add(data);
                    }
                }

                Assert.AreEqual(5, result.Count, "Expecting only 5 dogs!");
            }
            catch
            {
                File.Delete(tempFileName);
                throw;
            }
        }

        [DataTestMethod]
        [DataRow(5, true)]
        [DataRow(8, false)]
        public void GetRecord_CanHandelClassWithOneProperty_DataRead(int expectedNumberOfRows, bool blankRowsAreReturnedAsNull)
        {
            var tempFileName = Path.GetTempFileName();
            try
            {
                using (var fs = File.Create(tempFileName))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine("Name");
                    sw.WriteLine("George");
                    sw.WriteLine("");
                    sw.WriteLine("Bob");
                    sw.WriteLine("Tom");
                    sw.WriteLine("   ");
                    sw.WriteLine("Ralph");
                    sw.WriteLine("James");
                    sw.WriteLine("   ");
                }

                var result = new List<ClassWithOneProperty>();
                using (var fs = File.OpenRead(tempFileName))
                using (var sr = new StreamReader(fs))
                {
                    var reader = new CsvReaderService<ClassWithOneProperty>(sr);
                    reader.Configuration.HasHeaderRow = true;
                    reader.Configuration.BlankRowsAreReturnedAsNull = blankRowsAreReturnedAsNull; 
                    while (reader.CanRead())
                    {
                        var data = reader.GetRecord();
                        if (data != null || blankRowsAreReturnedAsNull == false)
                            result.Add(data);
                    }
                }

                Assert.AreEqual(expectedNumberOfRows, result.Count, $"Expecting {expectedNumberOfRows} names!");
            }
            catch
            {
                File.Delete(tempFileName);
                throw;
            }
        }
    }
}
