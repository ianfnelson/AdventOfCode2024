namespace AdventOfCode2024.Days;

public class Day01 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var lists = ParseInput(inputData);
        
        lists.Item1.Sort();
        lists.Item2.Sort();

        return lists.Item1
            .Select((t, i) => Math.Abs(t - lists.Item2[i]))
            .Sum()
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var lists = ParseInput(inputData);
        
        /* This optimization cuts the time taken from ~40ms to ~2ms on my MacBook,
         * compared to a naive approach where we scan the second list every time
         * we consider each entry in the first list.
         */
        var list2Counts = lists
            .Item2
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());
        
        return lists.Item1
            .Sum(id => list2Counts.GetValueOrDefault(id, 0) * id)
            .ToString();
    }

    private static Tuple<List<int>, List<int>> ParseInput(IEnumerable<string> inputData)
    {
        var leftList = new List<int>();
        var rightList = new List<int>();

        foreach (var line in inputData)
        {
            var ids = line.Split("   ");
            leftList.Add(int.Parse(ids[0]));
            rightList.Add(int.Parse(ids[1]));
        }
        
        return new Tuple<List<int>, List<int>>(leftList, rightList);
    }

    public override int Day => 1;
}