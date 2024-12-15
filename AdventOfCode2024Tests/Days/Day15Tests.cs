using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day15Tests
{
    private readonly Day15 _systemUnderTest = new();

    [TestCase("15a.txt", "2028")]
    [TestCase("15b.txt", "10092")]
    public void Part1Test(string filename, string expected)
    {
        Assert.That(_systemUnderTest.Part1($"TestData/{filename}"), Is.EqualTo(expected));
    }
    
    [Test]
    public void Part2Test()
    {
        Assert.That(_systemUnderTest.Part2($"TestData/15b.txt"), Is.EqualTo("9021"));
    }
}