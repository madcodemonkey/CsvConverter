using CsvConverter;
using SimpleDotNetConsoleExample2.Model;

string storageDirectory = "c:\\temp";
if (Directory.Exists(storageDirectory) == false)
    Directory.CreateDirectory(storageDirectory);

string squirrelFileName = Path.Combine(storageDirectory, "Squirrels.csv");

List<Squirrel> originalSquirrelList = CreateSquirrels(15);

ShowSquirrels("Generated file", originalSquirrelList);
WriteSquirrels(originalSquirrelList, squirrelFileName);

List<Squirrel> readSquirrelList = ReadSquirrels(squirrelFileName);
ShowSquirrels("From file", readSquirrelList);

Console.WriteLine("Hit enter to exit");
Console.ReadLine();


static void ShowSquirrels(string title, List<Squirrel> squirrelList)
{
    Console.WriteLine(title);
    foreach (var squirrel in squirrelList)
    {
        Console.WriteLine(squirrel.ToString());
    }
}

static List<Squirrel> ReadSquirrels(string squirrelFileName)
{
    var result = new List<Squirrel>();

    using (var fs = File.OpenRead(squirrelFileName))
    using (var sr = new StreamReader(fs))
    {
        ICsvReaderService<Squirrel> reader = new CsvReaderService<Squirrel>(sr);
        reader.Configuration.HasHeaderRow = true;
        reader.Configuration.BlankRowsAreReturnedAsNull = true;
        while (reader.CanRead())
        {
            Squirrel item = reader.GetRecord();
            if (item != null)
                result.Add(item);
        }
    }

    return result;
}

static void WriteSquirrels(List<Squirrel> originalSquirrelList, string squirrelFileName)
{
    using (var fs = File.Create(squirrelFileName))
    using (var sw = new StreamWriter(fs))
    {
        fs.Seek(0, SeekOrigin.Begin);
        ICsvWriterService<Squirrel> writer = new CsvWriterService<Squirrel>(sw);
        writer.Configuration.HasHeaderRow = true;
        foreach (var squirrel in originalSquirrelList)
        {
            writer.WriteRecord(squirrel);
        }
    }
}

static List<Squirrel> CreateSquirrels(int numberOfSquirrelsToCreate)
{
    Random rand = new Random(DateTime.Now.Second);
    var result = new List<Squirrel>();
    for (int i = 1; i <= numberOfSquirrelsToCreate; i++)
    {
        var oneSquirrel = new Squirrel()
        {
            Name = $" Squirrel Number {i} ",
            Age = i == 2 ? 2 : rand.Next(1, 5),
        };

        SetRandomSpecies(rand, oneSquirrel);
        result.Add(oneSquirrel);
    }
    return result;
}

static void SetRandomSpecies(Random rand, Squirrel oneSquirrel)
{
    switch (rand.Next(1, 5))
    {
        case 1:
            oneSquirrel.Species = " Fox";
            oneSquirrel.HairColor = "  Gray and red ";
            break;
        case 2:
            oneSquirrel.Species = " Red";
            oneSquirrel.HairColor = "  Red ";
            break;
        case 3:
            oneSquirrel.Species = " Southern Flying";
            oneSquirrel.HairColor = "  Gray and black";
            break;
        case 4:
            oneSquirrel.Species = " Rock";
            oneSquirrel.HairColor = "  Gray to brownish ";
            break;
        default:
            oneSquirrel.Species = " Gray";
            oneSquirrel.HairColor = "  Gray and white ";
            break;
    }
}
