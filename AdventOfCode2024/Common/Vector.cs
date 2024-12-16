namespace AdventOfCode2024.Common;

public class Vector(Coordinate coordinate, Direction direction)
{
    public Coordinate Coordinate { get; set; } = coordinate;
    public Direction Direction { get; private set; } = direction;

    public void Rotate90Clockwise()
    {
        Direction = Direction.Rotate90Clockwise();
    }
    
    public void Rotate90CounterClockwise()
    {
        Direction = Direction.Rotate90CounterClockwise();
    }
}