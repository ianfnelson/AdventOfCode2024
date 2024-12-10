using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day10Tests
{
    private readonly Day10 _systemUnderTest = new();
    
    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/10.txt"), Is.EqualTo("36"));
    } 
}