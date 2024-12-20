﻿using System.Text;

namespace CsvConverter.Core.IntegrationTests;

[TestClass]
public class GraduationDataTest : TestBase
{
    [TestMethod]
    public void CanReadAndWriteData()
    {
        string fileName = GetTestFileNameAndPath("TestFiles\\Graduation.csv");

        var studentList = new List<Graduation>();

        using (var fs = File.OpenRead(fileName))
        using (var sr = new StreamReader(fs, Encoding.Default))
        {
            var csv = new CsvReaderService<Graduation>(sr);
            csv.Configuration.HasHeaderRow = true;
            csv.Configuration.BlankRowsAreReturnedAsNull = true;

            while (csv.CanRead())
            {
                studentList.Add(csv.GetRecord());
            }
        }

        // Create a temp file
        var directoryName = Path.GetDirectoryName(fileName) ?? throw new ArgumentException("No directory name specified.");
        string tempFileName = Path.Combine(
            directoryName,
            Path.GetFileNameWithoutExtension(fileName) + "Temp.csv");

        using (var fs = File.Create(tempFileName))
        using (var sw = new StreamWriter(fs, Encoding.Default))
        {
            var service = new CsvWriterService<Graduation>(sw);
            service.Configuration.HasHeaderRow = true;
            foreach (var student in studentList)
            {
                service.WriteRecord(student);
            }
        }

        Assert.IsTrue(FilesAreEqual(fileName, tempFileName));
    }

    private static bool FilesAreEqual(string fileName, string tempFileName)
    {
        bool areEqual = true;
        using (var fs1 = File.OpenRead(fileName))
        using (var sr1 = new StreamReader(fs1, Encoding.Default))
        using (var fs2 = File.OpenRead(tempFileName))
        using (var sr2 = new StreamReader(fs2, Encoding.Default))
        {
            while (sr1.EndOfStream == false && sr2.EndOfStream == false)
            {
                var sr1Line = sr1.ReadLine();
                var sr2Line = sr2.ReadLine();

                if (sr1Line != sr2Line)
                {
                    Assert.Fail($"'{sr1Line}' ----- is not equal to '{sr2Line}'");
                    areEqual = false;
                    break;
                }
            }

            // Does sr1 or sr2 still have data to read?  If so, one file is longer than the other.
            if (areEqual && (sr1.EndOfStream == false || sr2.EndOfStream == false))
            {

                areEqual = false;
            }

        }

        return areEqual;
    }
}