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
        throw new NotImplementedException();
    }

    public override int Day => 11;

    public class Stones(string input)
    {
        private readonly List<long> _stones = input
            .Split(" ")
            .Select(long.Parse)
            .ToList();

        public void Blink(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Blink();
            }
        }
        
        public void Blink()
        {
            for (int i = _stones.Count-1; i >=0; i--)
            {
                string stoneString = _stones[i].ToString();
                if (stoneString == "0")
                {
                    _stones[i] = 1L;
                } else if (stoneString.Length % 2 == 0)
                {
                    var halfLength = stoneString.Length / 2;
                    _stones[i] = long.Parse(stoneString[..halfLength]);
                    _stones.Insert(i+1, long.Parse(stoneString[halfLength..]));
                }
                else
                {
                    _stones[i] *= 2024;
                }
            }
        }

        public int Count => _stones.Count;
    }
}