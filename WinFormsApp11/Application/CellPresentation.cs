using WinFormsApp11.Game;

namespace WinFormsApp11.Application;

public sealed record CellPresentation(
    Position Position,
    Piece? Piece,
    bool IsDarkSquare,
    bool IsSelectable,
    bool IsSelected,
    bool IsTarget);
