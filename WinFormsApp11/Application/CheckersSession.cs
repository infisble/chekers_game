using WinFormsApp11.Game;

namespace WinFormsApp11.Application;

public sealed class CheckersSession
{
    private readonly CheckersGame _game;

    private Position? _selectedPosition;
    private List<Move> _selectedMoves = new();
    private string _statusText;

    public CheckersSession(CheckersGame? game = null)
    {
        _game = game ?? new CheckersGame();
        _statusText = $"Turn: {ColorText(_game.CurrentPlayer)}";
    }

    public void Reset()
    {
        _game.Reset();
        _selectedPosition = null;
        _selectedMoves = new List<Move>();
        _statusText = $"Turn: {ColorText(_game.CurrentPlayer)}";
    }

    public void ClickCell(Position position)
    {
        if (_game.IsFinished)
        {
            return;
        }

        var selectedMove = _selectedMoves.FirstOrDefault(move => move.To == position);
        if (_selectedPosition.HasValue && selectedMove is not null)
        {
            ApplySelectedMove(selectedMove);
            return;
        }

        var piece = _game.Board.GetPiece(position);
        var selectable = _game.GetSelectablePieces();
        if (piece.HasValue && piece.Value.Color == _game.CurrentPlayer && selectable.Contains(position))
        {
            SelectPiece(position);
            var captureOnly = _selectedMoves.Any(static move => move.IsCapture);
            _statusText = captureOnly
                ? $"Piece selected: mandatory capture ({_selectedMoves.Count} options)"
                : $"Piece selected: {_selectedMoves.Count} moves available";
            return;
        }

        ClearSelection();
        _statusText = $"Turn: {ColorText(_game.CurrentPlayer)}";
    }

    public BoardPresentation BuildPresentation()
    {
        var cells = new List<CellPresentation>(Position.BoardSize * Position.BoardSize);
        var selectableSet = _game.GetSelectablePieces().ToHashSet();
        var targetSet = _selectedMoves.Select(static move => move.To).ToHashSet();

        for (var row = 0; row < Position.BoardSize; row++)
        {
            for (var column = 0; column < Position.BoardSize; column++)
            {
                var position = new Position(row, column);
                var isDarkSquare = (row + column) % 2 == 1;
                var piece = _game.Board.GetPiece(position);

                cells.Add(new CellPresentation(
                    position,
                    piece,
                    isDarkSquare,
                    selectableSet.Contains(position),
                    _selectedPosition.HasValue && _selectedPosition.Value == position,
                    targetSet.Contains(position)));
            }
        }

        return new BoardPresentation(
            cells,
            _game.CurrentPlayer,
            _game.Winner,
            _game.IsFinished,
            _statusText);
    }

    private void ApplySelectedMove(Move selectedMove)
    {
        if (!_game.ApplyMove(selectedMove))
        {
            return;
        }

        if (_game.IsFinished)
        {
            ClearSelection();
            _statusText = $"Victory: {ColorText(_game.Winner!.Value)}";
            return;
        }

        if (_game.ForcedCapturePiece.HasValue)
        {
            SelectPiece(_game.ForcedCapturePiece.Value);
            _statusText = $"Continue capture: {ColorText(_game.CurrentPlayer)}";
            return;
        }

        ClearSelection();
        _statusText = $"Turn: {ColorText(_game.CurrentPlayer)}";
    }

    private void SelectPiece(Position position)
    {
        _selectedPosition = position;
        _selectedMoves = _game.GetLegalMoves(position).ToList();
    }

    private void ClearSelection()
    {
        _selectedPosition = null;
        _selectedMoves = new List<Move>();
    }

    private static string ColorText(PieceColor color)
    {
        return color == PieceColor.White ? "white" : "black";
    }
}
