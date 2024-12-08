namespace AdventOfCode2024.Common;

public class Vector(Coordinate coordinate, Direction direction)
{
    public Coordinate Coordinate { get; set; } = coordinate;
    public Direction Direction { get; private set; } = direction;

    public void Rotate90()
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