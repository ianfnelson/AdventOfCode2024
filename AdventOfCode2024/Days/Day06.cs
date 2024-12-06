namespace AdventOfCode2024.Days;

public class Day06 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var map = new Map(inputData.ToList());
        
        map.Patrol();
        
        return map.Positions.Count(x => x.Value.IsVisited).ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var inputList = inputData.ToList();
        var map = new Map(inputList);

        var infiniteLoopCount = 0;

        foreach (var position in map.Positions.Values)
        {
            if (position.IsObstacle || position.Coordinates == map.GuardVector.Coordinates) continue;
            
            var newMap = new Map(inputList);
            newMap.Positions[position.Coordinates].IsObstacle = true;

            try
            {
                newMap.Patrol();
            }
            catch (InfinitePatrolException e)
            {
                infiniteLoopCount++;
            }
        }
        
        return infiniteLoopCount.ToString();
    }

    public override int Day => 6;
    
    private class Map
    {
        public Dictionary<Coordinates, Position> Positions { get; } = new();

        public Vector GuardVector { get;  } = null!;
        
        public void Patrol()
        {
            do
            {
                if (!Positions[GuardVector.Coordinates].VisitedDirections.Add(GuardVector.Direction))
                {
                    throw new InfinitePatrolException();
                }
                
                if (!Positions.TryGetValue(GuardVector.Coordinates.Move(GuardVector.Direction), out var nextPosition))
                {
                    break;
                }

                if (nextPosition.IsObstacle)
                {
                    GuardVector.Rotate();
                }
                else
                {
                    GuardVector.Coordinates = nextPosition.Coordinates;
                }
            } while(true);
        }
        
        public Map(IList<string> inputData)
        {
            for (var y = 0; y < inputData.Count; y++)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var character = inputData[y][x];

                    var position = new Position(x, y, character == '#');

                    if (character == '^')
                    {
                        GuardVector = new Vector(position.Coordinates);
                    }

                    Positions.Add(position.Coordinates, position);
                }
            }
        }

        public class Vector(Coordinates coordinates)
        {
            public Coordinates Coordinates { get; set; } = coordinates;
            public Direction Direction { get; private set; } = Direction.North;

            public void Rotate()
            {
                Direction = Direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West => Direction.North,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        public readonly record struct Coordinates(int x, int y)
        {
            private int X { get; } = x;
            private int Y { get; } = y;

            public Coordinates Move(Direction direction)
            {
                return direction switch
                {
                    Direction.North => new Coordinates(X, Y - 1),
                    Direction.East => new Coordinates(X + 1, Y),
                    Direction.South => new Coordinates(X, Y + 1),
                    Direction.West => new Coordinates(X - 1, Y),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }
        }

        public class Position(int x, int y, bool isObstacle)
        {
            public HashSet<Direction> VisitedDirections { get; } = new();
            
            public Coordinates Coordinates { get; set; } = new(x, y);
            public bool IsObstacle { get; set; } = isObstacle;

            public bool IsVisited => VisitedDirections.Count != 0;
        }
        
        public enum Direction
        {
            North,
            East,
            South,
            West
        }
    }

    public class InfinitePatrolException : Exception;
}