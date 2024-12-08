using AdventOfCode2024.Common;

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
        var map = new Map(inputData.ToList());
        
        map.CalculateSignalImpact(true);
        
        return map
            .Positions
            .Count(x => x.Value.IsAntinode)
            .ToString();
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

        public void CalculateSignalImpact(bool isPart2 = false)
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
                        CalculateTransmitterImpact(t1, t2.Coordinate - t1.Coordinate, isPart2);
                        CalculateTransmitterImpact(t2, t1.Coordinate - t2.Coordinate, isPart2);
                    }
                }
            }
        }

        private void CalculateTransmitterImpact(Position t, Coordinate direction, bool isPart2)
        {
            var p = t;
            
            if (isPart2) { p.IsAntinode = true; }

            do
            {
                var c0 = p.Coordinate - direction;
                p = Positions.GetValueOrDefault(c0);

                if (p != null)
                {
                    p.IsAntinode = true;
                }
            } while (p != null && isPart2);
        }
        
        public Dictionary<Coordinate, Position> Positions { get; set; } = new();
        
        public class Position(int x, int y, char? transmitter)
        {
            public Coordinate Coordinate { get; } = new(x, y);
            
            public char? Transmitter { get; set; } = transmitter;
            
            public bool IsAntinode { get; set; }
        }
    }

    public override int Day => 8;
}