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
        var puzzle = new Puzzle(inputData.ToList(), true);
        
        puzzle.DoPuzzle();

        return puzzle.SumOfBoxesGpsCoordinates.ToString();
    }

    public override int Day => 15;

    public class Puzzle
    {
        public Puzzle(IList<string> inputData, bool doubleWidth = false)
        {
            var y = 0;

            while (inputData[y].Length > 0)
            {
                var inputLine = doubleWidth ? WidenMapInput(inputData[y]) : inputData[y];
                
                for (var x = 0; x < inputLine.Length; x++)
                {
                    var contents = ParseContents(inputLine[x]);
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

        private static string WidenMapInput(string input)
        {
            return input
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.");
        }

        private static Contents ParseContents(char character)
        {
            return character switch
            {
                '#' => Contents.Wall,
                'O' => Contents.Box,
                '.' => Contents.Space,
                '@' => Contents.Robot,
                '[' => Contents.BoxLeft,
                ']' => Contents.BoxRight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void DoPuzzle()
        {
            var position = Map.Values.Single(x => x.Contents == Contents.Robot);

            Moves.Aggregate(position, Move);
        }

        private Position Move(Position position, Direction direction)
        {
            // Determine if we can move forward, and any boxes that will be pushed
            if (!CanMoveForward(position, direction, out var movedItems))
            {
                // We cannot move. Stay in the same position.
                return position;
            }

            // Push any boxes necessary.
            PushBoxes(movedItems, direction);
            
            // Determine the next position for the robot, and back-fill current position with a space.
            var nextPosition = Map[position.Coordinate.Move(direction)];
            position.Contents = Contents.Space;
            nextPosition.Contents = Contents.Robot;
            
            return nextPosition;
        }

        private bool CanMoveForward(Position position, Direction direction, out List<Position> pushedBoxes)
        {
            pushedBoxes = [];

            var positions = new List<Position> { position };
            
            do
            {
                positions = GetNextBlocksOrWall(positions, direction);

                if (!positions.Any())
                {
                    return true;
                }

                if (positions.Any(x => x.Contents == Contents.Wall))
                {
                    return false;
                }
                
                pushedBoxes.AddRange(positions);
            } while (true);
        }

        private List<Position> GetNextBlocksOrWall(IList<Position> positions, Direction direction)
        {
            var nextPositions = new List<Position>();

            foreach (var position in positions)
            {
                var nextPosition = Map[position.Coordinate.Move(direction)];
                
                nextPositions.Add(nextPosition);
                
                if (direction is Direction.South or Direction.North)
                {
                    switch (nextPosition.Contents)
                    {
                        case Contents.BoxRight:
                            nextPositions.Add(Map[nextPosition.Coordinate.Move(Direction.West)]);
                            break;
                        case Contents.BoxLeft:
                            nextPositions.Add(Map[nextPosition.Coordinate.Move(Direction.East)]);
                            break;
                    }
                }
            }

            return nextPositions.Where(x => x.Contents != Contents.Space).ToList();
        }

        private void PushBoxes(IList<Position> boxes, Direction direction)
        {
            var tuples = boxes
                .Select(x => new Tuple<Coordinate, Contents>(x.Coordinate, x.Contents))
                .Reverse()
                .ToList();

            foreach (var tuple in tuples)
            {
                Map[tuple.Item1.Move(direction)].Contents = tuple.Item2;
                Map[tuple.Item1].Contents = Contents.Space;
            }
        }

        public List<Direction> Moves { get; } = new();

        public Dictionary<Coordinate, Position> Map { get; } = new();

        public int SumOfBoxesGpsCoordinates =>
            Map
                .Values
                .Where(x => x.Contents is Contents.Box or Contents.BoxLeft)
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
            Space,
            BoxLeft,
            BoxRight
        }
    }
}