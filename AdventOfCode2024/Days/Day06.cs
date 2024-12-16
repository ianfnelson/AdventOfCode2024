using AdventOfCode2024.Common;

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
        var guardStartPoint = map.GuardVector.Coordinate;
        map.Patrol();

        var infiniteLoopCount = 0;

        foreach (var position in map.Positions.Values)
        {
            if (position.IsObstacle || position.Coordinate == guardStartPoint) continue;
            if (!map.Positions[position.Coordinate].IsVisited) continue;
            
            var newMap = new Map(inputList);
            newMap.Positions[position.Coordinate].IsObstacle = true;

            try
            {
                newMap.Patrol();
            }
            catch (InfinitePatrolException)
            {
                infiniteLoopCount++;
            }
        }
        
        return infiniteLoopCount.ToString();
    }

    public override int Day => 6;
    
    private class Map
    {
        public Dictionary<Coordinate, Position> Positions { get; } = new();

        public Vector GuardVector { get; private set; }
        
        public void Patrol()
        {
            do
            {
                if (!Positions[GuardVector.Coordinate].VisitedDirections.Add(GuardVector.Direction))
                {
                    throw new InfinitePatrolException();
                }
                
                if (!Positions.TryGetValue(GuardVector.Coordinate.Move(GuardVector.Direction), out var nextPosition))
                {
                    break;
                }

                GuardVector = nextPosition.IsObstacle ? 
                    GuardVector with { Direction = GuardVector.Direction.Rotate90Clockwise() } : 
                    GuardVector with { Coordinate = nextPosition.Coordinate };
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
                        GuardVector = new Vector(position.Coordinate, Direction.North);
                    }

                    Positions.Add(position.Coordinate, position);
                }
            }
        }

        public class Position(int x, int y, bool isObstacle)
        {
            public HashSet<Direction> VisitedDirections { get; } = new();
            public Coordinate Coordinate { get; set; } = new(x, y);
            public bool IsObstacle { get; set; } = isObstacle;

            public bool IsVisited => VisitedDirections.Count != 0;
        }
    }

    public class InfinitePatrolException : Exception;
}