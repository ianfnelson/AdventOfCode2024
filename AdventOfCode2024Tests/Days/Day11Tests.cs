using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day11Tests
{
    private readonly Day11 _systemUnderTest = new();
    
    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1(new List<string>{"125 17"}), Is.EqualTo("36"));
    } 
    
    // [Test]
    // public void Part2Test()
    // {
    //     Assert.That(_systemUnderTest.Part2("TestData/10.txt"), Is.EqualTo("81"));
    // } 
}