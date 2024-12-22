namespace AdventOfCode2024.Days;

public class Day22 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return inputData
            .Select(x => long.Parse(x))
            .Select(x => SecretNumberGenerator.GenerateNth(x, 2000))
            .Sum()
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public class SecretNumberGenerator
    {
        public static long GenerateNext(long input)
        {
            return Step3(Step2(Step1(input)));
        }

        public static long GenerateNth(long input, int n)
        {
            for (var i = 0; i < n; i++)
            {
                input = GenerateNext(input);
            }

            return input;
        }
        
        private static long Step1(long input)
        {
            return Prune(Mix(input, input * 64));
        }

        private static long Step2(long input)
        {
            return Prune(Mix(input, input / 32));
        }

        private static long Step3(long input)
        {
            return Prune(Mix(input, input * 2048));
        }

        private static long Mix(long secret, long additive)
        {
            return secret ^ additive;
        }

        private static long Prune(long secret)
        {
            return secret % 16777216L;
        }
    }
    
    public override int Day => 22;
}
