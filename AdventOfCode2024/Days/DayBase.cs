namespace AdventOfCode2024.Days;

public abstract class DayBase : IDay
{
    public abstract string Part1(IEnumerable<string> inputData);
    public abstract string Part2(IEnumerable<string> inputData);

    public abstract int Day { get; }

    public string Part1(string path)
    {
        return Part1(File.ReadLines(path));
    }

    public string Part2(string path)
    {
        return Part2(File.ReadLines(path));
    }
}