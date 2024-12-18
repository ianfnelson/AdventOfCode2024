using AdventOfCode2024.Common;
using InvalidOperationException = System.InvalidOperationException;

namespace AdventOfCode2024.Days;

public class Day18 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList(), 70);

        return puzzle.ShortestPathAfter(1024).ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList(), 70);

        var firstCutOff = puzzle.FirstCutOff();
        
        return $"{firstCutOff.X},{firstCutOff.Y}";
    }

    public override int Day => 18;

    public class Puzzle
    {
        public Puzzle(IEnumerable<string> inputData, int max)
        {
            for (var x = 0; x <= max; x++)
            {
                for (var y = 0; y <= max; y++)
                {
                    var position = new Position(x, y);
                    _map.Add(position.Coordinate, position);
                }
            }

            _map[new Coordinate(0, 0)].IsStart = true;
            _map[new Coordinate(max, max)].IsEnd = true;

            foreach (var c in inputData)
            {
                var coord = c.Split(",").Select(int.Parse).ToArray();
                _bytes.Add(new Coordinate(coord[0], coord[1]));
            }
        }

        private readonly Dictionary<Coordinate, Position> _map = new();

        private readonly List<Coordinate> _bytes = new();

        public Coordinate FirstCutOff()
        {
            var left = 0;
            var right = _bytes.Count - 1;

            while (left < right)
            {
                var mid = left + (right - left) / 2;

                try
                {
                    ShortestPathAfter(mid + 1);
                }
                catch (InvalidOperationException)
                {
                    right = mid;
                    continue;
                }

                left = mid + 1;
            }

            return _bytes[left];
        }
        
        public int ShortestPathAfter(int bytes)
        {
            SetCorruptedPositions(bytes);
            
            var queue = new Queue<(Position position, int distance)>();
            var visited = new HashSet<Coordinate>();

            var start = _map.Single(x => x.Value.IsStart).Value;
            
            queue.Enqueue((start, 0));
            visited.Add(start.Coordinate);

            while (queue.Count > 0)
            {
                var (current, distance) = queue.Dequeue();
                
                if (current.IsEnd)
                {
                    return distance;
                }

                var directions = new[] { Direction.North, Direction.East, Direction.South, Direction.West };
                
                foreach (var direction in directions)
                {
                    var neighbour = current.Coordinate.Move(direction);

                    if (_map.ContainsKey(neighbour) && !visited.Contains(neighbour) && !_map[neighbour].IsCorrupted)
                    {
                        visited.Add(neighbour);
                        queue.Enqueue((_map[neighbour], distance + 1));
                    }
                }
            }

            throw new InvalidOperationException("No solutions to the maze");
        }

        private void SetCorruptedPositions(int bytes)
        {
            foreach (var position in _map.Values)
            {
                position.IsCorrupted = false;
            }
            
            for (int i = 0; i < bytes; i++)
            {
                _map[_bytes[i]].IsCorrupted = true;
            }
        }
        
        public class Position(int x, int y)
        {
            public bool IsCorrupted { get; set; }

            public Coordinate Coordinate { get; set; } = new Coordinate(x, y);
            
            public bool IsStart { get; set; }
            
            public bool IsEnd { get; set; }
        }
    }
}