namespace AdventOfCode2024.Days;

public class Day08 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var map = new Map(inputData.ToList());
        
        map.CalculateSignalImpact();
        
        return map
            .Positions
            .Count(x => x.Value.IsAntinode)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public class Map
    {
        public Map(IList<string> inputData)
        {
            for (var y = 0; y < inputData.Count; y++)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var character = inputData[y][x];
                    
                    var transmitter = character == '.' ? (char?)null : character;
                    
                    var position = new Position(x, y, transmitter);
                    
                    Positions.Add(position.Coordinate, position);
                }
            }
        }

        public void CalculateSignalImpact()
        {
            var transmitterGroups = Positions
                .Values
                .Where(x => x.Transmitter.HasValue)
                .GroupBy(x => x.Transmitter!.Value);

            foreach (var transmitterGroup in transmitterGroups)
            {
                var transmitters = transmitterGroup.ToList();
                for (var i = 0; i < transmitters.Count; i++)
                {
                    for (var j = i + 1; j < transmitters.Count; j++)
                    {
                        var t1 = transmitters[i];
                        var t2 = transmitters[j];
                        
                        var direction = t2.Coordinate - t1.Coordinate;
                        
                        var c0 = t1.Coordinate - direction;
                        var c3 = t2.Coordinate + direction;
                        
                        if (Positions.TryGetValue(c0, out var p0))
                        {
                            p0.IsAntinode = true;
                        }

                        if (Positions.TryGetValue(c3, out var p3))
                        {
                            p3.IsAntinode = true;
                        }
                    }
                }
            }
        }
        
        public Dictionary<Coordinate, Position> Positions { get; set; } = new();
        
        public class Position(int x, int y, char? transmitter)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public char? Transmitter { get; set; } = transmitter;
            
            public bool IsAntinode { get; set; }
        }
        
        public readonly record struct Coordinate(int x, int y)
        {
            private int X { get; } = x;
            private int Y { get; } = y;
            
            public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
            public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);
        }
    }

    public override int Day => 8;
}