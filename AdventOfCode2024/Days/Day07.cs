namespace AdventOfCode2024.Days;

public class Day07 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return inputData
            .Select(x => new Equation(x))
            .Where(e => e.CouldBeTrue)
            .Sum(e => e.TestValue)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
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

        public bool CouldBeTrue
        {
            get
            {
                foreach (var operatorCombination in OperatorCombinations(Inputs.Count - 1))
                {
                    long total = Inputs[0];

                    for (var i = 0; i < Inputs.Count-1; i++)
                    {
                        if (operatorCombination[i] == Operator.Addition)
                        {
                            total += Inputs[i + 1];
                        }
                        else
                        {
                            total *= Inputs[i + 1];
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
        }
        
        private static IEnumerable<List<Operator>> OperatorCombinations(int n)
        {
            var totalCombinations = 1 << n; // 2^n combinations

            for (var i = 0; i < totalCombinations; i++)
            {
                var combination = new List<Operator>();
                for (var j = 0; j < n; j++)
                {
                    // Check if the j-th bit of i is set
                    var isAddition = (i & (1 << j)) != 0;
                    combination.Add(isAddition ? Operator.Addition : Operator.Multiplication);
                }
                yield return combination;
            }
        }
        
        enum Operator
        {
            Addition,
            Multiplication
        }
    }

    public override int Day => 7;
}