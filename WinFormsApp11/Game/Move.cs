namespace WinFormsApp11.Game;

public sealed record Move(Position From, Position To, IReadOnlyList<Position> CapturedPositions)
{
    public bool IsCapture => CapturedPositions.Count > 0;
}
