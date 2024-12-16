using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day16 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());

        var start = new Vector(puzzle.Map.Single(x => x.Value.IsStart).Key, Direction.East);

        return puzzle.MinimalPathToEnd(start).ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public override int Day => 16;

    public class Puzzle
    {
        public Puzzle(IList<string> inputData)
        {
            for (var y = 0; y < inputData.Count; y++)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var c = inputData[y][x];

                    if (c == '#') continue;

                    var position = new Position(x, y)
                    {
                        IsStart = c == 'S',
                        IsEnd = c == 'E'
                    };
                    
                    Map.Add(position.Coordinate, position);
                }
            }
        }

        public Dictionary<Coordinate, Position> Map = new();

        public int MinimalPathToEnd(Vector from)
        {
            return MinimalPathToEnd(from, new HashSet<Coordinate>()).Result;
        }
        
        public async Task<int> MinimalPathToEnd(Vector from, ISet<Coordinate> visited)
        {
            // If we've reached the end, return zero.
            if (Map[from.Coordinate].IsEnd) return await Task.FromResult(0);

            visited.Add(from.Coordinate);
            
            var tasks = new List<Task<int>>();

            var d1 = from.Direction.Rotate90CounterClockwise();
            var v1 = new Vector(from.Coordinate.Move(d1), d1);
            if (Map.ContainsKey(v1.Coordinate) && !visited.Contains(v1.Coordinate))
            {
                tasks.Add(MinimalPathToEnd(v1, new HashSet<Coordinate>(visited)).ContinueWith(t => 1001 + t.Result));
            }
            
            var d2 = from.Direction.Rotate90Clockwise();
            var v2 = new Vector(from.Coordinate.Move(d2), d2);
            if (Map.ContainsKey(v2.Coordinate) && !visited.Contains(v2.Coordinate))
            {
                tasks.Add(MinimalPathToEnd(v2, new HashSet<Coordinate>(visited)).ContinueWith(t => 1001 + t.Result));
            }

            var d3 = from.Direction;
            var v3 = new Vector(from.Coordinate.Move(d3), d3);
            if (Map.ContainsKey(v3.Coordinate) && !visited.Contains(v3.Coordinate))
            {
                tasks.Add(MinimalPathToEnd(v3, new HashSet<Coordinate>(visited)).ContinueWith(t => 1 + t.Result));
            }

            if (tasks.Count == 0)
            {
                return int.MaxValue / 2;
            }

            return (await Task.WhenAll(tasks)).Min();
        }

        public class Position(int x, int y)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public bool IsStart { get; set; }
            
            public bool IsEnd { get; set; }
        }
    }
}