using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day12 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        return new Garden(inputData.ToList())
            .CalculateRegions()
            .Sum(x => x.Price)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        return new Garden(inputData.ToList())
            .CalculateRegions()
            .Sum(x => x.DiscountPrice)
            .ToString();
    }

    public override int Day => 12;

    public class Garden
    {
        public Garden(IList<string> inputData)
        {
            for (var y = 0; y < inputData.Count; y++)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var plot = new Plot(x, y, inputData[y][x]);
                    Plots.Add(plot.Coordinate, plot);
                }
            }
        }

        public Dictionary<Coordinate, Plot> Plots { get; }= new();

        public IEnumerable<Region> CalculateRegions()
        {
            foreach (var plot in Plots.Values)
            {
                if (!plot.Visited)
                {
                    yield return GetRegionContaining(plot);
                }
            }
        }

        private Region GetRegionContaining(Plot plot)
        {
            plot.Visited = true;
            var stack = new Stack<Plot>();
            stack.Push(plot);
            var area = 0;
            var perimeter = 0;
            var corners = 0;

            var directions = new[] { Direction.North, Direction.East, Direction.South, Direction.West };

            while (stack.Count > 0)
            {
                var p1 = stack.Pop();
                area++;
                
                foreach (var direction in directions)
                {
                    corners += IsClockwiseCorner(p1, direction) ? 1 : 0;
                    
                    if (!Plots.TryGetValue(p1.Coordinate.Move(direction), out var p2) || p2.Plant != p1.Plant)
                    {
                        perimeter++;
                    } else if (!p2.Visited)
                    {
                        p2.Visited = true;
                        stack.Push(p2);
                    }
                }
            }

            return new Region(area, perimeter, corners, plot.Plant);
        }

        private bool IsClockwiseCorner(Plot p1, Direction direction)
        {
            var clockwiseDirection = direction.Rotate90Clockwise();

            if (!Plots.TryGetValue(p1.Coordinate.Move(direction), out var p2) || p2.Plant != p1.Plant)
            {
                if (IsConvexCorner(p1, clockwiseDirection)) return true;
            }
            else
            {
                if (IsConcaveCorner(p1, direction, clockwiseDirection)) return true;
            }

            return false;
        }

        private bool IsConcaveCorner(Plot p1, Direction direction, Direction clockwiseDirection)
        {
            return Plots.TryGetValue(p1.Coordinate.Move(clockwiseDirection), out var p2)
                   && p2.Plant == p1.Plant
                   && Plots[p2.Coordinate.Move(direction)].Plant != p1.Plant;
        }

        private bool IsConvexCorner(Plot p1, Direction clockwiseDirection)
        {
            return !Plots.TryGetValue(p1.Coordinate.Move(clockwiseDirection), out var p2) || p2.Plant != p1.Plant;
        }

        public class Plot(int x, int y, char plant)
        {
            public Coordinate Coordinate { get; } = new(x, y);

            public char Plant { get; } = plant;
            
            public bool Visited { get; set; }
        }
        
        public class Region(int area, int perimeter, int sides, char plant)
        {
            public int Area { get; } = area;
            
            public int Perimeter { get; } = perimeter;
            
            public char Plant { get; } = plant;

            public int Sides { get; } = sides;
            
            public int Price => Area * Perimeter;

            public int DiscountPrice => Area * Sides;
        }
    }
}