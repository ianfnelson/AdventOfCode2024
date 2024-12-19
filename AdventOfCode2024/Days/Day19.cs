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
        throw new NotImplementedException();
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
            return _designs.Count(IsDesignPossible);
        }

        private bool IsDesignPossible(string design)
        {
            var memo = new Dictionary<int, bool>();
            
            return IsPartialDesignPossible(0, memo, design);
        }

        private bool IsPartialDesignPossible(int position, Dictionary<int, bool> memo, string design)
        {
            if (memo.TryGetValue(position, out var possible))
            {
                return possible;
            }

            if (position == design.Length)
            {
                return true;
            }

            foreach (var towel in _towels)
            {
                if (position + towel.Length <= design.Length &&
                    design[position..].StartsWith(towel))
                {
                    if (IsPartialDesignPossible(position + towel.Length, memo, design))
                    {
                        memo[position] = true;
                        return true;
                    }
                }
            }

            memo[position] = false;
            return false;
        }
    }
    
}