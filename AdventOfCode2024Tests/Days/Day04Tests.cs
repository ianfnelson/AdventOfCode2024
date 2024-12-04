using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day04Tests
{
    private readonly Day04 _systemUnderTest = new();
    
    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/4.txt"), Is.EqualTo("18"));
    }
    
    [Test]
    public void Part2Test()
    {
        Assert.That(_systemUnderTest.Part2("TestData/4.txt"), Is.EqualTo("9"));
    }
}