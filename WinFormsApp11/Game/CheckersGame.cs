namespace WinFormsApp11.Game;

public sealed class CheckersGame
{
    private static readonly (int Row, int Column)[] Diagonals =
    {
        (-1, -1),
        (-1, 1),
        (1, -1),
        (1, 1)
    };

    private readonly CheckersBoard _board = new();
    private Position? _forcedCapturePiece;

    public CheckersGame()
    {
        Reset();
    }

    public CheckersBoard Board => _board;

    public PieceColor CurrentPlayer { get; private set; }

    public Position? ForcedCapturePiece => _forcedCapturePiece;

    public PieceColor? Winner { get; private set; }

    public bool IsFinished => Winner.HasValue;

    public void Reset()
    {
        _board.ResetToClassicSetup();
        CurrentPlayer = PieceColor.White;
        Winner = null;
        _forcedCapturePiece = null;
    }

    public void LoadPosition(
        IEnumerable<(Position Position, Piece Piece)> pieces,
        PieceColor currentPlayer,
        Position? forcedCapturePiece = null)
    {
        _board.Clear();

        foreach (var (position, piece) in pieces)
        {
            if (!position.IsInsideBoard)
            {
                throw new ArgumentOutOfRangeException(nameof(pieces), "One or more positions are outside of the board.");
            }

            _board.SetPiece(position, piece);
        }

        CurrentPlayer = currentPlayer;
        Winner = null;
        _forcedCapturePiece = forcedCapturePiece;
    }

    public IReadOnlyList<Position> GetSelectablePieces()
    {
        if (IsFinished)
        {
            return Array.Empty<Position>();
        }

        var captureMoves = GetAllCaptureMoves(CurrentPlayer);
        if (captureMoves.Count > 0)
        {
            return captureMoves
                .Select(static move => move.From)
                .Distinct()
                .OrderBy(static position => position.Row)
                .ThenBy(static position => position.Column)
                .ToList();
        }

        var selectable = new List<Position>();
        foreach (var (position, piece) in _board.GetPieces(CurrentPlayer))
        {
            if (GenerateQuietMoves(position, piece).Count > 0)
            {
                selectable.Add(position);
            }
        }

        return selectable;
    }

    public IReadOnlyList<Move> GetLegalMoves(Position from)
    {
        if (IsFinished)
        {
            return Array.Empty<Move>();
        }

        var selectedPiece = _board.GetPiece(from);
        if (!selectedPiece.HasValue || selectedPiece.Value.Color != CurrentPlayer)
        {
            return Array.Empty<Move>();
        }

        if (_forcedCapturePiece.HasValue && _forcedCapturePiece.Value != from)
        {
            return Array.Empty<Move>();
        }

        var allCaptureMoves = GetAllCaptureMoves(CurrentPlayer);
        if (allCaptureMoves.Count > 0)
        {
            return allCaptureMoves.Where(move => move.From == from).ToList();
        }

        return GenerateQuietMoves(from, selectedPiece.Value);
    }

    public bool ApplyMove(Move move)
    {
        if (IsFinished)
        {
            return false;
        }

        var legalMoves = GetLegalMoves(move.From);
        var matchedMove = legalMoves.FirstOrDefault(existing => existing.To == move.To);
        if (matchedMove is null)
        {
            return false;
        }

        ApplyValidatedMove(matchedMove);
        return true;
    }

    private void ApplyValidatedMove(Move move)
    {
        var piece = _board.GetPiece(move.From);
        if (!piece.HasValue)
        {
            throw new InvalidOperationException("Cannot apply a move from an empty square.");
        }

        _board.SetPiece(move.From, null);
        _board.SetPiece(move.To, piece);

        foreach (var capturedPosition in move.CapturedPositions)
        {
            _board.SetPiece(capturedPosition, null);
        }

        var movedPiece = _board.GetPiece(move.To);
        if (!movedPiece.HasValue)
        {
            throw new InvalidOperationException("Move application failed to place the piece.");
        }

        if (ShouldPromote(movedPiece.Value, move.To.Row))
        {
            movedPiece = movedPiece.Value.Promote();
            _board.SetPiece(move.To, movedPiece);
        }

        var opponent = GetOpponent(CurrentPlayer);
        if (_board.CountPieces(opponent) == 0)
        {
            Winner = CurrentPlayer;
            _forcedCapturePiece = null;
            return;
        }

        if (move.IsCapture)
        {
            var continuationMoves = GenerateCaptureMoves(move.To, movedPiece.Value);
            if (continuationMoves.Count > 0)
            {
                _forcedCapturePiece = move.To;
                return;
            }
        }

        _forcedCapturePiece = null;
        CurrentPlayer = opponent;

        if (!HasAnyLegalMove(CurrentPlayer))
        {
            Winner = GetOpponent(CurrentPlayer);
        }
    }

    private bool HasAnyLegalMove(PieceColor player)
    {
        var captureMoves = GetAllCaptureMoves(player);
        if (captureMoves.Count > 0)
        {
            return true;
        }

        foreach (var (position, piece) in _board.GetPieces(player))
        {
            if (GenerateQuietMoves(position, piece).Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    private List<Move> GetAllCaptureMoves(PieceColor player)
    {
        if (_forcedCapturePiece.HasValue)
        {
            var forcedPiece = _board.GetPiece(_forcedCapturePiece.Value);
            if (forcedPiece.HasValue && forcedPiece.Value.Color == player)
            {
                return GenerateCaptureMoves(_forcedCapturePiece.Value, forcedPiece.Value);
            }

            return new List<Move>();
        }

        var moves = new List<Move>();
        foreach (var (position, piece) in _board.GetPieces(player))
        {
            moves.AddRange(GenerateCaptureMoves(position, piece));
        }

        return moves;
    }

    private List<Move> GenerateQuietMoves(Position from, Piece piece)
    {
        return piece.Kind switch
        {
            PieceKind.Man => GenerateManQuietMoves(from, piece.Color),
            PieceKind.King => GenerateKingQuietMoves(from),
            _ => throw new ArgumentOutOfRangeException(nameof(piece), piece.Kind, "Unknown piece kind")
        };
    }

    private List<Move> GenerateCaptureMoves(Position from, Piece piece)
    {
        return piece.Kind switch
        {
            PieceKind.Man => GenerateManCaptureMoves(from, piece.Color),
            PieceKind.King => GenerateKingCaptureMoves(from, piece.Color),
            _ => throw new ArgumentOutOfRangeException(nameof(piece), piece.Kind, "Unknown piece kind")
        };
    }

    private List<Move> GenerateManQuietMoves(Position from, PieceColor color)
    {
        var direction = color == PieceColor.White ? 1 : -1;
        var moves = new List<Move>();

        foreach (var columnOffset in new[] { -1, 1 })
        {
            var target = from.Offset(direction, columnOffset);
            if (target.IsInsideBoard && !_board.GetPiece(target).HasValue)
            {
                moves.Add(new Move(from, target, Array.Empty<Position>()));
            }
        }

        return moves;
    }

    private List<Move> GenerateKingQuietMoves(Position from)
    {
        var moves = new List<Move>();

        foreach (var (rowStep, columnStep) in Diagonals)
        {
            var current = from.Offset(rowStep, columnStep);
            while (current.IsInsideBoard)
            {
                if (_board.GetPiece(current).HasValue)
                {
                    break;
                }

                moves.Add(new Move(from, current, Array.Empty<Position>()));
                current = current.Offset(rowStep, columnStep);
            }
        }

        return moves;
    }

    private List<Move> GenerateManCaptureMoves(Position from, PieceColor color)
    {
        var moves = new List<Move>();

        foreach (var (rowStep, columnStep) in Diagonals)
        {
            var adjacent = from.Offset(rowStep, columnStep);
            var landing = from.Offset(rowStep * 2, columnStep * 2);

            if (!adjacent.IsInsideBoard || !landing.IsInsideBoard)
            {
                continue;
            }

            var adjacentPiece = _board.GetPiece(adjacent);
            if (!adjacentPiece.HasValue || adjacentPiece.Value.Color == color)
            {
                continue;
            }

            if (_board.GetPiece(landing).HasValue)
            {
                continue;
            }

            moves.Add(new Move(from, landing, new[] { adjacent }));
        }

        return moves;
    }

    private List<Move> GenerateKingCaptureMoves(Position from, PieceColor color)
    {
        var moves = new List<Move>();

        foreach (var (rowStep, columnStep) in Diagonals)
        {
            Position? capturedPiecePosition = null;
            var current = from.Offset(rowStep, columnStep);

            while (current.IsInsideBoard)
            {
                var currentPiece = _board.GetPiece(current);
                if (!currentPiece.HasValue)
                {
                    if (capturedPiecePosition.HasValue)
                    {
                        moves.Add(new Move(from, current, new[] { capturedPiecePosition.Value }));
                    }

                    current = current.Offset(rowStep, columnStep);
                    continue;
                }

                if (currentPiece.Value.Color == color)
                {
                    break;
                }

                if (capturedPiecePosition.HasValue)
                {
                    break;
                }

                capturedPiecePosition = current;
                current = current.Offset(rowStep, columnStep);
            }
        }

        return moves;
    }

    private static PieceColor GetOpponent(PieceColor color)
    {
        return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    private static bool ShouldPromote(Piece piece, int row)
    {
        if (piece.Kind == PieceKind.King)
        {
            return false;
        }

        return (piece.Color == PieceColor.White && row == Position.BoardSize - 1)
            || (piece.Color == PieceColor.Black && row == 0);
    }
}
