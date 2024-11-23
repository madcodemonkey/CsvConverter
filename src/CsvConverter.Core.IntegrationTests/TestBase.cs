using System.Reflection;

namespace CsvConverter.Core.IntegrationTests;

public class TestBase
{
    protected string GetTestFileNameAndPath(string partialPath)
    {
        string someDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                               throw new ArgumentException("Unable to determine assembly location.");
        var parentDir = Directory.GetParent(someDirectory) ??
                        throw new ArgumentException("Unable to determine parent directory.");

        var parentFullName = parentDir.Parent?.Parent?.Parent?.FullName ??
                             throw new ArgumentException("Unable to determine parent directory.");
        string dataFileName = Path.Combine(parentFullName, partialPath);

        if (File.Exists(dataFileName) == false)
            Assert.Fail("could not find " + dataFileName);

        return dataFileName;
    }
}