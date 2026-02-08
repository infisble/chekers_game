using WinFormsApp11.Game;

namespace WinFormsApp11.Application;

public sealed record BoardPresentation(
    IReadOnlyList<CellPresentation> Cells,
    PieceColor CurrentPlayer,
    PieceColor? Winner,
    bool IsFinished,
    string StatusText);
