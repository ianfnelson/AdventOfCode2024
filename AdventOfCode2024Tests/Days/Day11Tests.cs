using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day11Tests
{
    private readonly Day11 _systemUnderTest = new();
    
    [TestCase("125 17",1, 3)]
    [TestCase("125 17",2, 4)]
    [TestCase("125 17",3, 5)]
    [TestCase("125 17",4, 9)]
    [TestCase("125 17",5, 13)]
    [TestCase("125 17",6, 22)]
    [TestCase("125 17",25, 55312)]
    public void Stones_Blink(string input, int blinkCount, int expectedStonesCount)
    {
        var stones = new Day11.Stones(input);
        
        stones.Blink(blinkCount);
        
        Assert.That(stones.Count, Is.EqualTo(expectedStonesCount));
    } 
}