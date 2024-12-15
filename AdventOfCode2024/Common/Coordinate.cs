namespace AdventOfCode2024.Common;

public readonly record struct Coordinate(int X, int Y)
{
    public Coordinate Move(Direction direction)
    {
        return direction switch
        {
            Direction.North => new Coordinate(X, Y - 1),
            Direction.East => new Coordinate(X + 1, Y),
            Direction.South => new Coordinate(X, Y + 1),
            Direction.West => new Coordinate(X - 1, Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
    
    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);
}