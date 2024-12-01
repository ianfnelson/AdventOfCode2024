using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day01Tests
{
    private readonly Day01 _systemUnderTest = new();

    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/1.txt"), Is.EqualTo("11"));
    }
    
    [Test]
    public void Part2Test()
    {
        Assert.That(_systemUnderTest.Part2("TestData/1.txt"), Is.EqualTo("31"));
    }
}