using AdventOfCode2024.Common;

namespace AdventOfCode2024.Days;

public class Day15 : DayBase
{
    protected override string Part1(IEnumerable<string> inputData)
    {
        var puzzle = new Puzzle(inputData.ToList());
        
        puzzle.DoPuzzle();

        return puzzle.SumOfBoxesGpsCoordinates.ToString();
    }

    protected override string Part2(IEnumerable<string> inputData)
    {
        throw new NotImplementedException();
    }

    public override int Day => 15;

    public class Puzzle
    {
        public Puzzle(IList<string> inputData)
        {
            var y = 0;

            while (inputData[y].Length > 0)
            {
                for (var x = 0; x < inputData[y].Length; x++)
                {
                    var contents = ParseContents(inputData[y][x]);
                    var position = new Position(x, y, contents);
                    Map.Add(position.Coordinate, position);
                }

                y++;
            }

            y++;

            while (y < inputData.Count)
            {
                for (var i = 0; i < inputData[y].Length; i++)
                {
                    Moves.Add(inputData[y][i].ParseDirection());
                }
                
                y++;
            }
        }

        private static Contents ParseContents(char character)
        {
            return character switch
            {
                '#' => Contents.Wall,
                'O' => Contents.Box,
                '.' => Contents.Space,
                '@' => Contents.Robot,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void DoPuzzle()
        {
            var position = Map.Values.Single(x => x.Contents == Contents.Robot);
            
            foreach (var move in Moves)
            {
                position = Move(position, move);
            }
        }

        private Position Move(Position position, Direction direction)
        {
            // Seek out the next space. Stop if we hit brick wall.
            var spaceOrWall = position;
            do
            {
                spaceOrWall = Map[spaceOrWall.Coordinate.Move(direction)];
            } while (spaceOrWall.Contents == Contents.Box);
            
            // If we hit a wall before finding a space, we cannot move.
            if (spaceOrWall.Contents == Contents.Wall)
            {
                return position;
            }
            
            // We found a space! The robot can move forward, leaving a space behind.
            var nextPosition = Map[position.Coordinate.Move(direction)];
            position.Contents = Contents.Space;
            nextPosition.Contents = Contents.Robot;
            
            // Move a box into the final position if necessary
            if (spaceOrWall.Coordinate != nextPosition.Coordinate)
            {
                spaceOrWall.Contents = Contents.Box;
            }

            return nextPosition;
        }

        public List<Direction> Moves { get; } = new();

        public Dictionary<Coordinate, Position> Map { get; } = new();

        public int SumOfBoxesGpsCoordinates =>
            Map
                .Values
                .Where(x => x.Contents == Contents.Box)
                .Sum(x => x.GpsCoordinate);

        public class Position(int x, int y, Contents contents)
        {
            public Coordinate Coordinate { get; } = new(x, y);

            public Contents Contents { get; set; } = contents;

            public int GpsCoordinate => 100 * Coordinate.Y + Coordinate.X;
        }

        public enum Contents
        {
            Wall,
            Box,
            Robot,
            Space
        }
    }
}