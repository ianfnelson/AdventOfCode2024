namespace AdventOfCode2024.Days;

public class Day19 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());

        return puzzle.CountPossibleDesigns().ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());

        return puzzle.CountWays().ToString();
    }

    public override int Day => 19;

    private class Puzzle(IList<string> inputData)
    {
        private readonly List<string> _towels = inputData[0].Split(", ").ToList();

        private readonly List<string> _designs = inputData.Skip(2).ToList();

        public int CountPossibleDesigns()
        {
            return _designs.Count(x => CountWays(x) > 0);
        }

        public long CountWays()
        {
            return _designs.Sum(CountWays);
        }

        private long CountWays(string design)
        {
            var memo = new Dictionary<int, long>();
            return CountWaysPartial(0, memo, design);
        }

        private long CountWaysPartial(int position, Dictionary<int, long> memo, string design)
        {
            if (memo.TryGetValue(position, out var possible))
            {
                return possible;
            }

            if (position == design.Length)
            {
                return 1L;
            }

            var ways = 0L;

            foreach (var towel in _towels)
            {
                if (position + towel.Length <= design.Length &&
                    design[position..].StartsWith(towel))
                {
                    ways += CountWaysPartial(position + towel.Length, memo, design);
                }
            }

            memo[position] = ways;
            return ways;
        }
    }
}