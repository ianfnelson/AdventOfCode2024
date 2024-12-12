using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day12 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var garden = new Garden(inputData.ToList());

        var regions = garden.CalculateRegions().ToList();

        return regions
            .Sum(x => x.Price)
            .ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
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
            var plant = plot.Plant;

            var directions = new[] { Direction.North, Direction.East, Direction.South, Direction.West };

            while (stack.Count > 0)
            {
                var p1 = stack.Pop();
                area++;
                
                foreach (var direction in directions)
                {
                    if (!Plots.TryGetValue(p1.Coordinate.Move(direction), out Plot? p2))
                    {
                        perimeter++;
                        continue;
                    }

                    if (p2.Plant == plant)
                    {
                        if (!p2.Visited)
                        {
                            p2.Visited = true;
                            stack.Push(p2);
                        }
                    }
                    else
                    {
                        perimeter++;
                    }
                }
            }

            return new Region(area, perimeter, plant);
        }
        
        public class Plot(int x, int y, char plant)
        {
            public Coordinate Coordinate { get; set; } = new Coordinate(x, y);

            public char Plant { get; set; } = plant;
            
            public bool Visited { get; set; }
        }
        
        public class Region(int area, int perimeter, char plant)
        {
            public int Area { get; set; } = area;

            public int Perimeter { get; set; } = perimeter;

            public char Plant { get; set; } = plant;
            
            public int Price => Area * Perimeter;
        }
    }
}