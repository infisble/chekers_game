namespace WinFormsApp11.Game;

public readonly record struct Piece(PieceColor Color, PieceKind Kind)
{
    public Piece Promote()
    {
        return Kind == PieceKind.King ? this : new Piece(Color, PieceKind.King);
    }
}
