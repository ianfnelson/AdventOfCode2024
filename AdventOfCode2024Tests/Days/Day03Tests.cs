using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day03Tests
{
    [Test]
    public void Section_Parse_Part1ResultAsExpected()
    {
        var section = Day03.Memory.Parse(["xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"]);
        
        Assert.That(section.Part1Result, Is.EqualTo(161));
    }
    
    [Test]
    public void Section_Parse_Part2ResultAsExpected()
    {
        var section = Day03.Memory.Parse(["xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"]);
        
        Assert.That(section.Part2Result, Is.EqualTo(48));
    }
}