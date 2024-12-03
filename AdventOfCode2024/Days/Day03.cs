using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day03 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return Memory.Parse(inputData)
            .Part1Result
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return Memory.Parse(inputData)
            .Part2Result
            .ToString();
    }
    
    public class Memory(IEnumerable<Memory.Multiplication> multiplications)
    {
        private IList<Multiplication> Multiplications { get; } = multiplications.ToList();

        public int Part1Result => Multiplications.Sum(x => x.Result);
        
        public int Part2Result => Multiplications.Where(x => x.IsEnabledForPart2).Sum(x => x.Result);
        
        public static Memory Parse(IEnumerable<string> input)
        {
            var multiplications = new List<Multiplication>();
            var isEnabledForPart2 = true;

            var regex = new Regex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
            
            foreach (var line in input)
            {
                var matches = regex.Matches(line);
                
                foreach (Match match in matches)
                {
                    switch (match.Value)
                    {
                        case "do()":
                            isEnabledForPart2 = true;
                            break;
                        case "don't()":
                            isEnabledForPart2 = false;
                            break;
                        default:
                        {
                            var x = int.Parse(match.Groups[1].Value);
                            var y = int.Parse(match.Groups[2].Value);
                            multiplications.Add(new Multiplication(x, y, isEnabledForPart2));
                            break;
                        }
                    }
                }
            }

            return new Memory(multiplications);
        }

        public class Multiplication(int x, int y, bool isEnabledForPart2)
        {
            public int X { get; } = x;

            public int Y { get;} = y;

            public int Result => X * Y;
            
            public bool IsEnabledForPart2 { get; } = isEnabledForPart2;
        }
    }

    public override int Day => 3;
}