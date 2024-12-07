namespace AdventOfCode2024.Days;

public class Day07 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return inputData
            .Select(x => new Equation(x))
            .Where(e => e.CouldBeTrue())
            .Sum(e => e.TestValue)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return inputData
            .Select(x => new Equation(x))
            .Where(e => e.CouldBeTrue(true))
            .Sum(e => e.TestValue)
            .ToString();
    }

    private class Equation
    {
        public Equation(string input)
        {
            var parts = input.Split(": ");
            TestValue = long.Parse(parts[0]);
            Inputs = parts[1].Split(" ").Select(int.Parse).ToList();
        }
        
        public long TestValue { get; }
        
        public IList<int> Inputs { get; }

        public bool CouldBeTrue(bool allowConcatenation = false)
        {
            foreach (var operatorCombination in OperatorCombinations(Inputs.Count - 1, allowConcatenation))
            {
                long total = Inputs[0];

                for (var i = 0; i < Inputs.Count-1; i++)
                {
                    if (operatorCombination[i] == Operator.Addition)
                    {
                        total += Inputs[i + 1];
                    }
                    else if (operatorCombination[i] == Operator.Multiplication)
                    {
                        total *= Inputs[i + 1];
                    } else if (operatorCombination[i] == Operator.Concatenation)
                    {
                        total = long.Parse($"{total}{Inputs[i + 1]}");
                    }

                    if (total > TestValue)
                    {
                        break;
                    }
                }

                if (total == TestValue)
                {
                    return true;
                }
            }

            return false;
        }
        
        private static IEnumerable<List<Operator>> OperatorCombinations(int n, bool allowConcatenation = false)
        {
            var operatorCount = allowConcatenation ? 3 : 2;
            
            var totalCombinations = (int)Math.Pow(operatorCount, n);

            for (var i = 0; i < totalCombinations; i++)
            {
                var combination = new List<Operator>();
                var current = i;

                for (var j = 0; j < n; j++)
                {
                    var operatorIndex = current % operatorCount;

                    var op = operatorIndex switch
                    {
                        0 => Operator.Addition,
                        1 => Operator.Multiplication,
                        2 => Operator.Concatenation,
                        _ => Operator.Addition
                    };

                    combination.Add(op);
                    current /= operatorCount;
                }

                yield return combination;
            }
        }
        
        enum Operator
        {
            Addition,
            Multiplication,
            Concatenation
        }
    }

    public override int Day => 7;
}