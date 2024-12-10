using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day10 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var map = new Map(inputData.ToList());

        var tasks = map
            .Positions
            .Where(x => x.Value.Elevation == 0)
            .Select(valley => map.SummitsReachableFrom(valley.Value))
            .ToList();

        var results = Task.WhenAll(tasks).Result;
        
        return results.Sum(x => x.Count).ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var map = new Map(inputData.ToList());

        var tasks = map
            .Positions
            .Where(x => x.Value.Elevation == 0)
            .Select(valley => map.CountTrailsToSummitFrom(valley.Value))
            .ToList();

        var results = Task.WhenAll(tasks).Result;

        return results.Sum().ToString();
    }

    public override int Day => 10;

    public class Map
    {
        public Dictionary<Coordinate, Position> Positions { get; set; } = new();
        
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

        public async Task<List<Coordinate>> SummitsReachableFrom(Position position)
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
                    tasks.Add(SummitsReachableFrom(nextPosition));
                }
            }

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(x => x).Distinct().ToList();
        }
        
        public async Task<int> CountTrailsToSummitFrom(Position position)
        {
            if (position.Elevation == 9)
            {
                return await Task.FromResult(1);
            }

            var tasks = new List<Task<int>> { Task.FromResult(0) };

            foreach (var direction in new[] { Direction.North, Direction.East, Direction.South, Direction.West })
            {
                if (!Positions.TryGetValue(position.Coordinate.Move(direction), out var nextPosition))
                {
                    continue;
                }
        
                if (nextPosition.Elevation == position.Elevation + 1)
                {
                    tasks.Add(CountTrailsToSummitFrom(nextPosition));
                }
            }

            var results = await Task.WhenAll(tasks);

            return results.Sum();
        }
        
        public class Position(int x, int y, int elevation)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public int Elevation { get; } = elevation;
        }
    }
}