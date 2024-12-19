namespace AdventOfCode2024.Days;

public class Day19 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());

        return puzzle.PossibleDesigns().ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());

        return puzzle.CountWays().ToString();
    }

    public override int Day => 19;

    public class Puzzle
    {
        public Puzzle(IList<string> inputData)
        {
            _towels = inputData[0].Split(", ").ToList();
            _designs = inputData.Skip(2).ToList();
        }
        
        private readonly List<string> _towels;

        private readonly List<string> _designs;

        public int PossibleDesigns()
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
            
            var answer= CountWaysPartial(0, memo, design);

            return answer;
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