using AdventOfCode2024.Common;
using AdventOfCode2024.Days;

namespace AdventOfCode2024Tests.Days;

[TestFixture]
public class Day18Tests
{
    [Test]
    public void Part1()
    {
        var inputData = File.ReadAllLines("TestData/18.txt");
        var puzzle = new Day18.Puzzle(inputData, 6);

        var shortestPath = puzzle.ShortestPathAfter(12);

        Assert.That(shortestPath, Is.EqualTo(22));
    }
    
    [Test]
    public void Part2()
    {
        var inputData = File.ReadAllLines("TestData/18.txt");
        var puzzle = new Day18.Puzzle(inputData, 6);

        var firstCutOff = puzzle.FirstCutOff();

        Assert.That(firstCutOff, Is.EqualTo(new Coordinate(6,1)));
    }
}