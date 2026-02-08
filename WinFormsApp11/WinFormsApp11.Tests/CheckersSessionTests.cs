using WinFormsApp11.Application;
using WinFormsApp11.Game;

namespace WinFormsApp11.Tests;

public sealed class CheckersSessionTests
{
    [Fact]
    public void InitialPresentation_HasExpectedDefaults()
    {
        var session = new CheckersSession();

        var state = session.BuildPresentation();

        Assert.False(state.IsFinished);
        Assert.Equal(PieceColor.White, state.CurrentPlayer);
        Assert.Equal("Turn: white", state.StatusText);
        Assert.Equal(64, state.Cells.Count);
        Assert.Equal(12, state.Cells.Count(cell => cell.Piece?.Color == PieceColor.White));
        Assert.Equal(12, state.Cells.Count(cell => cell.Piece?.Color == PieceColor.Black));
    }

    [Fact]
    public void ClickSelectablePiece_MarksSelectionAndTargets()
    {
        var session = new CheckersSession();

        session.ClickCell(new Position(2, 1));
        var state = session.BuildPresentation();

        Assert.Contains("Piece selected", state.StatusText);
        Assert.Contains(state.Cells, cell => cell.Position == new Position(2, 1) && cell.IsSelected);
        Assert.Contains(state.Cells, cell => cell.IsTarget);
    }

    [Fact]
    public void MultiCaptureFlow_KeepsSamePlayerAndRequestsContinuation()
    {
        var game = new CheckersGame();
        game.LoadPosition(
            new[]
            {
                (new Position(2, 1), new Piece(PieceColor.White, PieceKind.Man)),
                (new Position(3, 2), new Piece(PieceColor.Black, PieceKind.Man)),
                (new Position(5, 4), new Piece(PieceColor.Black, PieceKind.Man)),
                (new Position(7, 0), new Piece(PieceColor.Black, PieceKind.Man))
            },
            PieceColor.White);

        var session = new CheckersSession(game);

        session.ClickCell(new Position(2, 1));
        session.ClickCell(new Position(4, 3));
        var state = session.BuildPresentation();

        Assert.Equal(PieceColor.White, state.CurrentPlayer);
        Assert.Equal("Continue capture: white", state.StatusText);
        Assert.Contains(state.Cells, cell => cell.Position == new Position(4, 3) && cell.IsSelected);
        Assert.Contains(state.Cells, cell => cell.Position == new Position(6, 5) && cell.IsTarget);
    }

    [Fact]
    public void LastCapture_EndsGameWithWinnerStatus()
    {
        var game = new CheckersGame();
        game.LoadPosition(
            new[]
            {
                (new Position(2, 1), new Piece(PieceColor.White, PieceKind.Man)),
                (new Position(3, 2), new Piece(PieceColor.Black, PieceKind.Man))
            },
            PieceColor.White);

        var session = new CheckersSession(game);

        session.ClickCell(new Position(2, 1));
        session.ClickCell(new Position(4, 3));
        var state = session.BuildPresentation();

        Assert.True(state.IsFinished);
        Assert.Equal(PieceColor.White, state.Winner);
        Assert.Equal("Victory: white", state.StatusText);
    }
}
