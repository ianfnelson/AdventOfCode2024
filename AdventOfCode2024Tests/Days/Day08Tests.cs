using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day08Tests
{
    private readonly Day08 _systemUnderTest = new();
    
    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/8.txt"), Is.EqualTo("14"));
    } 
    
    [Test]
    public void Part2Test()
    {
        Assert.That(_systemUnderTest.Part2("TestData/8.txt"), Is.EqualTo("34"));
    } 
}