using System.Collections.Immutable;

namespace AdventOfCode2024.Days;

public class Day02 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return inputData
            .Select(Report.Parse)
            .Count(report => report.IsSafe)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return inputData
            .Select(Report.Parse)
            .Count(report => report.IsSafeWithDampener)
            .ToString();
    }

    public class Report
    {
        public Report(IEnumerable<int> levels)
        {
            Levels = levels.ToImmutableList();
            
            Deltas = Levels
                .Zip(Levels.Skip(1), (first, second) => second - first)
                .ToImmutableList();
        }
        
        public ImmutableList<int> Levels { get; }

        private ImmutableList<int> Deltas { get; }
        
        public bool IsSafe =>
            LevelsAreIncreasingOrDecreasing &&
            LevelsDoNotChangeByMoreThanThree;

        public bool IsSafeWithDampener =>
            IsSafe ||
            GenerateReportsWithoutOneLevel().Any(r => r.IsSafe);

        private IEnumerable<Report> GenerateReportsWithoutOneLevel()
        {
            for (var i = 0; i < Levels.Count; i++)
            {
                var levelsWithoutOne = new List<int>(Levels);
                levelsWithoutOne.RemoveAt(i);
                yield return new Report(levelsWithoutOne);
            }
        }
        
        private bool LevelsAreIncreasingOrDecreasing =>
            Deltas.All(delta => delta > 0) ||
            Deltas.All(delta => delta < 0);

        private bool LevelsDoNotChangeByMoreThanThree =>
            Deltas.All(delta => delta >= -3) &&
            Deltas.All(delta => delta <= 3);

        public static Report Parse(string input)
        {
            return new Report(input.Split(' ').Select(int.Parse));
        }
    }

    public override int Day => 2;
}