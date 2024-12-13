using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

public class Day13 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
       return ParseInput(inputData.ToList())
            .Select(x => x.Solve())
            .Where(x => x.HasSolution)
            .Sum(x => x.Tokens)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var equations = ParseInput(inputData.ToList(), 10000000000000L).ToList();
        
        return equations
            .Select(x => x.Solve())
            .Where(x => x.HasSolution)
            .Sum(x => x.Tokens)
            .ToString();
    }

    public override int Day => 13;

    private static IEnumerable<SimultaneousEquation> ParseInput(IList<string> inputData, long prizeError = 0L)
    {
        var numbers = new Regex(@"(\d+)");
        
        for (var i = 0; i < inputData.Count; i+=4)
        {
            var matchesA = numbers.Matches(inputData[i]);
            var a1 = int.Parse(matchesA[0].Value);
            var a2 = int.Parse(matchesA[1].Value);
            
            var matchesB = numbers.Matches(inputData[i+1]);
            var b1 = int.Parse(matchesB[0].Value);
            var b2 = int.Parse(matchesB[1].Value);
            
            var matchesC = numbers.Matches(inputData[i+2]);
            var c1 = prizeError + int.Parse(matchesC[0].Value);
            var c2 = prizeError + int.Parse(matchesC[1].Value);

            yield return new SimultaneousEquation(a1, b1, c1, a2, b2, c2);
        }
    }

    public class SimultaneousEquation(int a1, int b1, long c1, int a2, int b2, long c2)
    {
        public Solution Solve()
        {
            // Cramer's Rule - https://en.wikipedia.org/wiki/Cramer%27s_rule
            double determinant = a1 * b2 - a2 * b1;

            if (determinant == 0)
            {
                return Solution.None;
            }

            var a = (c1 * b2 - c2 * b1) / determinant;
            var b = (a1 * c2 - a2 * c1) / determinant;

            return new Solution(a, b);
        }

        public class Solution
        {
            private Solution(){}
            
            public Solution(double a, double b)
            {
                A = a;
                B = b;

                HasSolution = 
                    A % 1 == 0 &&
                    B % 1 == 0;
            }

            public static Solution None => new();

            public double A { get; }
            
            public double B { get; }
            
            public bool HasSolution { get; }

            public long Tokens => !HasSolution ? 0 : 3 * (long)A + (long)B;
        }
    }
}