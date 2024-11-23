using System.Text;

namespace CsvConverter.Core.IntegrationTests;

[TestClass]
public class CustomAttributeTests : TestBase
{
    [TestMethod]
    public void CanReadAndWriteData()
    {
        string fileName = GetTestFileNameAndPath("TestFiles\\CustomAttributeTests.csv");

        var dataList = new List<CustomAttributeData>();

        using (var fs = File.OpenRead(fileName))
        using (var sr = new StreamReader(fs, Encoding.Default))
        {
            var csv = new CsvReaderService<CustomAttributeData>(sr);
            csv.Configuration.HasHeaderRow = true;
            csv.Configuration.BlankRowsAreReturnedAsNull = true;
            csv.Configuration.ThrowExceptionIfColumnCountChanges = false;

            while (csv.CanRead())
            {
                dataList.Add(csv.GetRecord());
            }
        }

        CustomAttributeData data1 = dataList.Single(w => w.Id == 1);
        Assert.IsNotNull(data1, "Could not find record 1");
        Assert.AreEqual(12, data1.Speed);
        CheckNumbers(data1.Items, 1, 2, 3);
        Assert.AreEqual("not-blank", data1.TextValue);

        CustomAttributeData data2 = dataList.Single(w => w.Id == 2);
        Assert.IsNotNull(data2, "Could not find record 2");
        Assert.AreEqual(78, data2.Speed);
        CheckNumbers(data2.Items, 4, 5, 6);
        Assert.AreEqual("Bob", data2.TextValue);


        CustomAttributeData data3 = dataList.Single(w => w.Id == 3);
        Assert.IsNotNull(data3, "Could not find record 3");
        Assert.AreEqual(134, data3.Speed);
        CheckNumbers(data3.Items, 7, 8, 9, 10);
        Assert.AreEqual("Bob", data3.TextValue);
    }

    private static void CheckNumbers(int[] numbers, params int[] expected)
    {
        Assert.AreEqual(expected.Length, numbers.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.AreEqual(expected[i], numbers[i]);
        }
    }
}