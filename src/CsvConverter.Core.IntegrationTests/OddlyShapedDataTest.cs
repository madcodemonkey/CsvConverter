using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvConverter.Core.IntegrationTests
{
    [TestClass]
    public class OddlyShapedDataTest
    {
        [TestMethod]
        public void CanReadAndWriteData()
        {
            string fileName = GetTestFileNameAndPath();

            var dataList = new List<OddlyShapedData>();

            using (var fs = File.OpenRead(fileName))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                var csv = new CsvReaderService<OddlyShapedData>(sr);
                csv.Configuration.HasHeaderRow = true;
                csv.Configuration.BlankRowsAreReturnedAsNull = true;
                csv.Configuration.ThrowExceptionIfColumnCountChanges = false;

                while (csv.CanRead())
                {
                    dataList.Add(csv.GetRecord());
                }
            }

            OddlyShapedData data1 = dataList.FirstOrDefault(w => w.FirstName == "David");
            Assert.IsNotNull(data1, "Could not find record with David Jackson");
            Assert.AreEqual("Jackson", data1.LastName);
            Assert.AreEqual(45, data1.Age);
            Assert.AreEqual(72, data1.HeightInInches);

            OddlyShapedData data2 = dataList.FirstOrDefault(w => w.FirstName == "Heather");
            Assert.IsNotNull(data2, "Could not find record with Heather Thomas");
            Assert.AreEqual("Thomas", data2.LastName);
            Assert.AreEqual(0, data2.Age);
            Assert.AreEqual(0, data2.HeightInInches);


            OddlyShapedData data3 = dataList.FirstOrDefault(w => w.FirstName == "Janet");
            Assert.IsNotNull(data3, "Could not find record with Janet Reno");
            Assert.AreEqual("Reno", data3.LastName);
            Assert.AreEqual(65, data3.Age);
            Assert.AreEqual(70, data3.HeightInInches);
        }


        private static string GetTestFileNameAndPath()
        {
            String someDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var parentDir = Directory.GetParent(someDirectory);

            string dataFileName = Path.Combine(parentDir.Parent.Parent.Parent.FullName, "TestFiles\\OddlyShapedData.csv");

            if (File.Exists(dataFileName) == false)
                Assert.Fail("counld not find " + dataFileName);

            return dataFileName;
        }
    }
}
