namespace AdventOfCode2024.Days;

public class Day11 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var stones = new Stones(inputData.Single());
        
        stones.Blink(25);
        
        return stones.Count.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var stones = new Stones(inputData.Single());
        
        stones.Blink(75);
        
        return stones.Count.ToString();
    }

    public override int Day => 11;

    public class Stones(string input)
    {
        private readonly Dictionary<long, long> _stoneCounts = input
            .Split(" ")
            .Select(long.Parse)
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => (long)x.Count());

        public void Blink(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Blink();
            }
        }
        
        private void Blink()
        {
            foreach (var stoneCount in _stoneCounts.ToList())
            {
                var changedStones = ChangeStone(stoneCount.Key);

                foreach (var stone in changedStones)
                {
                    if (_stoneCounts.ContainsKey(stone))
                    {
                        _stoneCounts[stone] += stoneCount.Value;
                    }
                    else
                    {
                        _stoneCounts[stone] = stoneCount.Value;
                    }
                }

                _stoneCounts[stoneCount.Key] -= stoneCount.Value;
            }
        }

        private static List<long> ChangeStone(long input)
        {
            if (input == 0L)
            {
                return [1L];
            }

            var stoneString = input.ToString();

            if (stoneString.Length % 2 == 0)
            {
                var halfLength = stoneString.Length / 2;
                var s1 = long.Parse(stoneString[..halfLength]);
                var s2 = long.Parse(stoneString[halfLength..]);

                return [s1, s2];
            }

            return [input * 2024];
        }
        
        public long Count => _stoneCounts.Sum(x => x.Value);
    }
}