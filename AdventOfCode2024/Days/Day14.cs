using System.Text.RegularExpressions;
using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day14 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var map = new Map(inputData, 103, 101);
        
        map.Wait(100);

        return map.SafetyFactor.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        var map = new Map(inputData, 103, 101);

        var seconds = 0;

        do
        {
            map.Wait();
            seconds++;
        } while (!map.IsPotentialTree);

        return seconds.ToString();
    }

    public override int Day => 14;

    public class Map()
    {
        public int Height { get; }
        public int Width { get; }
        public List<Robot> Robots { get; } = [];
        
        public Map(IEnumerable<string> inputData, int height, int width) : this()
        {
            Height = height;
            Width = width;
            
            var numbers = new Regex(@"-?(\d+)");
            
            foreach (var line in inputData)
            {
                var matches = numbers.Matches(line);
                
                Robots.Add(new Robot(
                    int.Parse(matches[0].Value),
                    int.Parse(matches[1].Value),
                    int.Parse(matches[2].Value),
                    int.Parse(matches[3].Value),
                    this
                    ));
            }
        }

        public void Wait(int seconds)
        {
            for (var i = 0; i < seconds; i++)
            {
                Wait();
            }
        }

        public void Wait()
        {
            foreach (var robot in Robots)
            {
                robot.Move();
            }
        }

        public int SafetyFactor
        {
            get
            {
                return Robots
                    .GroupBy(r => DetermineQuadrant(r.Position))
                    .Where(g => g.Key != "Middle")
                    .Select(g => g.Count())
                    .Aggregate(1, (a, b) => a * b);
            }
        }

        public bool IsPotentialTree => Adjacency > 0.5D;

        private string DetermineQuadrant(Coordinate coordinate)
        {
            var midX = (Width - 1) / 2;
            var midY = (Height - 1) / 2;
            
            if (coordinate.X < midX && coordinate.Y < midY) return "NW";
            if (coordinate.X > midX && coordinate.Y < midY) return "NE";
            if (coordinate.X < midX && coordinate.Y > midY) return "SW";
            if (coordinate.X > midX && coordinate.Y > midY) return "SE";

            return "Middle";
        }

        private double Adjacency
        {
            get
            {
                var directions = new[] { Direction.East, Direction.North, Direction.South, Direction.West };

                var occupiedPositions = Robots
                    .Select(x => x.Position)
                    .Distinct()
                    .ToHashSet();

                var adjacentPositions = occupiedPositions
                    .Count(position => directions.Any(d => occupiedPositions.Contains(position.Move(d))));

                return (double)adjacentPositions / Robots.Count;
            }
        }

        public class Robot (int px, int py, int vx, int vy, Map map)
        {
            public Coordinate Position { get; set; } = new(px, py);
            public Coordinate Velocity { get; } = new(vx, vy);
            
            public void Move()
            {
                var position = Position + Velocity;
                var nx = position.X;
                var ny = position.Y;

                if (nx >= map.Width)
                {
                    nx -= map.Width;
                }
                
                if (ny >= map.Height)
                {
                    ny -= map.Height;
                }
                
                if (nx < 0)
                {
                    nx += map.Width;
                }
                
                if (ny < 0)
                {
                    ny += map.Height;
                }

                Position = new Coordinate(nx, ny);
            }
        }
    }
}