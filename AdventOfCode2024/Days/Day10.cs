using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day10 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return new Map(inputData.ToList())
            .SummitTrails()
            .Sum(x => x.Distinct().Count())
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return new Map(inputData.ToList())
            .SummitTrails()
            .Sum(x => x.Count)
            .ToString();
    }

    public override int Day => 10;

    private class Map
    {
        private Dictionary<Coordinate, Position> Positions { get; } = new();
        
        public Map(IList<string> inputData)
        {
            for (var y = 0; y < inputData.Count; y++)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var position = new Position(x, y, int.Parse(inputData[y][x].ToString()));
                    Positions.Add(position.Coordinate, position);
                }
            }
        }

        public List<Coordinate>[] SummitTrails()
        {
            var tasks = Positions
                .Where(x => x.Value.Elevation == 0)
                .Select(valley => SummitTrails(valley.Value))
                .ToList();

            return Task.WhenAll(tasks).Result;
        }

        private async Task<List<Coordinate>> SummitTrails(Position position)
        {
            if (position.Elevation == 9)
            {
                return await Task.FromResult(new List<Coordinate> {position.Coordinate});
            }

            var tasks = new List<Task<List<Coordinate>>>();

            foreach (var direction in new[] { Direction.North, Direction.East, Direction.South, Direction.West })
            {
                if (!Positions.TryGetValue(position.Coordinate.Move(direction), out var nextPosition))
                {
                    continue;
                }
                
                if (nextPosition.Elevation == position.Elevation + 1)
                {
                    tasks.Add(SummitTrails(nextPosition));
                }
            }

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(x => x).ToList();
        }

        private class Position(int x, int y, int elevation)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public int Elevation { get; } = elevation;
        }
    }
}