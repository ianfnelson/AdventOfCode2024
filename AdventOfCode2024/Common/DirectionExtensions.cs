namespace AdventOfCode2024.Common;

public static class DirectionExtensions {
    public static Direction Rotate90(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Direction ParseDirection(this char character)
    {
        return character switch
        {
            '^' => Direction.North,
            'v' => Direction.South,
            '<' => Direction.West,
            '>' => Direction.East,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}