using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day12Tests
{
    private readonly Day12 _systemUnderTest = new();

    [TestCase("12a.txt", "140")]
    [TestCase("12b.txt", "772")]
    [TestCase("12c.txt", "1930")]
    public void Part1Test(string filename, string expected)
    {
        Assert.That(_systemUnderTest.Part1($"TestData/{filename}"), Is.EqualTo(expected));
    }
    
    [TestCase("12a.txt", "80")]
    [TestCase("12b.txt", "436")]
    [TestCase("12c.txt", "1206")]
    [TestCase("12d.txt", "236")]
    [TestCase("12e.txt", "368")]
    public void Part2Test(string filename, string expected)
    {
        Assert.That(_systemUnderTest.Part2($"TestData/{filename}"), Is.EqualTo(expected));
    }
}