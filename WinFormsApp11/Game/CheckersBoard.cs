namespace WinFormsApp11.Game;

public sealed class CheckersBoard
{
    private readonly Piece?[,] _cells = new Piece?[Position.BoardSize, Position.BoardSize];

    public void Clear()
    {
        Array.Clear(_cells, 0, _cells.Length);
    }

    public Piece? GetPiece(Position position)
    {
        return position.IsInsideBoard ? _cells[position.Row, position.Column] : null;
    }

    public void SetPiece(Position position, Piece? piece)
    {
        if (!position.IsInsideBoard)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        _cells[position.Row, position.Column] = piece;
    }

    public IEnumerable<(Position Position, Piece Piece)> GetPieces(PieceColor? color = null)
    {
        for (var row = 0; row < Position.BoardSize; row++)
        {
            for (var column = 0; column < Position.BoardSize; column++)
            {
                var piece = _cells[row, column];
                if (!piece.HasValue)
                {
                    continue;
                }

                if (color.HasValue && piece.Value.Color != color.Value)
                {
                    continue;
                }

                yield return (new Position(row, column), piece.Value);
            }
        }
    }

    public int CountPieces(PieceColor color)
    {
        var count = 0;
        foreach (var (_, piece) in GetPieces(color))
        {
            if (piece.Color == color)
            {
                count++;
            }
        }

        return count;
    }

    public void ResetToClassicSetup()
    {
        Clear();

        for (var row = 0; row < Position.BoardSize; row++)
        {
            for (var column = 0; column < Position.BoardSize; column++)
            {
                var isDarkSquare = (row + column) % 2 == 1;
                if (!isDarkSquare)
                {
                    continue;
                }

                if (row <= 2)
                {
                    _cells[row, column] = new Piece(PieceColor.White, PieceKind.Man);
                }
                else if (row >= 5)
                {
                    _cells[row, column] = new Piece(PieceColor.Black, PieceKind.Man);
                }
            }
        }
    }
}
