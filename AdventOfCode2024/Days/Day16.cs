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
            var queue = new List<(Vector v, int cost)>();

            var visited = new HashSet<Vector>();

            queue.Add((from, 0));

            while (queue.Count > 0)
            {
                queue.Sort(Comparer<(Vector v, int cost)>.Create((a,b) => a.cost.CompareTo(b.cost)));

                var (currentVector, currentCost) = queue[0];
                queue.RemoveAt(0);
                
                Console.WriteLine("Queue Length {0}, X {1} Y {2}", queue.Count, currentVector.Coordinate.X, currentVector.Coordinate.Y);

                if (!Map.TryGetValue(currentVector.Coordinate, out var position)) continue;
                if (position.IsEnd) return currentCost;

                if (!visited.Add(currentVector)) continue;
                
                Console.WriteLine("Visited count {0}", visited.Count);

                foreach (var nextVector in GetMoves(currentVector)
                             .Where(m => Map.ContainsKey(m.Coordinate))
                             .Where(m => !visited.Contains(m)))
                {
                    var cost = currentCost + (nextVector.Direction == currentVector.Direction ? 1 : 1001);
                    queue.Add((nextVector, cost));
                }
            }

            throw new InvalidOperationException("No solutions to the maze");
        }

        private static IEnumerable<Vector> GetMoves(Vector v)
        {
            var d1 = v.Direction.Rotate90CounterClockwise();
            var v1 = new Vector(v.Coordinate.Move(d1), d1);
            yield return v1;
            
            var d2 = v.Direction.Rotate90Clockwise();
            var v2 = new Vector(v.Coordinate.Move(d2), d2);
            yield return v2;

            var d3 = v.Direction;
            var v3 = new Vector(v.Coordinate.Move(d3), d3);
            yield return v3;
        }
        
        public class Position(int x, int y)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public bool IsStart { get; set; }
            
            public bool IsEnd { get; set; }
        }
    }
}