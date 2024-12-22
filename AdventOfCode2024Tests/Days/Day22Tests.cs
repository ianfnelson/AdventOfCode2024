using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day22Tests
{
    private readonly Day22 _systemUnderTest = new();
    
    [TestCase(123, 15887950)]
    [TestCase(15887950, 16495136)]
    [TestCase(16495136, 527345)]
    [TestCase(527345, 704524)]
    [TestCase(704524, 1553684)]
    [TestCase(1553684, 12683156)]
    [TestCase(12683156, 11100544)]
    [TestCase(11100544, 12249484)]
    [TestCase(12249484, 7753432)]
    [TestCase(7753432, 5908254)]
    public void SecretNumberGenerator_GenerateNext(long input, long expected)
    {
        var generator = new Day22.SecretNumberGenerator();

        var next = Day22.SecretNumberGenerator.GenerateNext(input);
        
        Assert.That(next, Is.EqualTo(expected));
    }
    
    [TestCase(1, 8685429)]
    [TestCase(10, 4700978)]
    [TestCase(100, 15273692)]
    [TestCase(2024, 8667524)]
    public void SecretNumberGenerator_GenerateNth(long input, long expected)
    {
        var generator = new Day22.SecretNumberGenerator();

        var nth = Day22.SecretNumberGenerator.GenerateNth(input, 2000);
        
        Assert.That(nth, Is.EqualTo(expected));
    }
    
    [Test]
    public void Part1Test()
    {
        Assert.That(_systemUnderTest.Part1("TestData/22.txt"), Is.EqualTo("37327623"));
    }
}