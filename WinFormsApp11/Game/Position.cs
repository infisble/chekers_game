namespace WinFormsApp11.Game;

public readonly record struct Position(int Row, int Column)
{
    public const int BoardSize = 8;

    public bool IsInsideBoard => Row >= 0 && Row < BoardSize && Column >= 0 && Column < BoardSize;

    public int ToIndex() => (Row * BoardSize) + Column;

    public static Position FromIndex(int index)
    {
        return new Position(index / BoardSize, index % BoardSize);
    }

    public Position Offset(int rowDelta, int columnDelta)
    {
        return new Position(Row + rowDelta, Column + columnDelta);
    }
}
