namespace AdventOfCode2024.Common;

public class Vector(Coordinate coordinate, Direction direction)
{
    public Coordinate Coordinate { get; set; } = coordinate;
    public Direction Direction { get; private set; } = direction;

    public void Rotate90()
    {
        Direction = Direction.Rotate90();
    }
}