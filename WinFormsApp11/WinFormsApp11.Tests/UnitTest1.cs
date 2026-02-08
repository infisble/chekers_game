using WinFormsApp11.Game;

namespace WinFormsApp11.Tests;

public sealed class CheckersGameTests
{
    [Fact]
    public void InitialPosition_HasClassicSetup()
    {
        var game = new CheckersGame();

        Assert.Equal(PieceColor.White, game.CurrentPlayer);
        Assert.Equal(12, game.Board.CountPieces(PieceColor.White));
        Assert.Equal(12, game.Board.CountPieces(PieceColor.Black));
        Assert.False(game.IsFinished);
    }

    [Fact]
    public void MandatoryCapture_BlocksQuietMovesForOtherPieces()
    {
        var game = new CheckersGame();
        game.LoadPosition(
            new[]
            {
                (new Position(2, 1), new Piece(PieceColor.White, PieceKind.Man)),
                (new Position(2, 5), new Piece(PieceColor.White, PieceKind.Man)),
                (new Position(3, 2), new Piece(PieceColor.Black, PieceKind.Man)),
                (new Position(7, 0), new Piece(PieceColor.Black, PieceKind.Man))
            },
            PieceColor.White);

        var selectable = game.GetSelectablePieces();
        Assert.Single(selectable);
        Assert.Contains(new Position(2, 1), selectable);
        Assert.Empty(game.GetLegalMoves(new Position(2, 5)));

        var captureMove = Assert.Single(game.GetLegalMoves(new Position(2, 1)));
        Assert.True(captureMove.IsCapture);
        Assert.Equal(new Position(4, 3), captureMove.To);
    }

    [Fact]
    public void MultiCapture_StaysOnSamePlayerUntilChainEnds()
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

        var firstCapture = Assert.Single(game.GetLegalMoves(new Position(2, 1)));
        Assert.True(game.ApplyMove(firstCapture));

        Assert.Equal(PieceColor.White, game.CurrentPlayer);
        Assert.Equal(new Position(4, 3), game.ForcedCapturePiece);

        var secondCapture = Assert.Single(game.GetLegalMoves(new Position(4, 3)));
        Assert.True(game.ApplyMove(secondCapture));

        Assert.Null(game.ForcedCapturePiece);
        Assert.Equal(PieceColor.Black, game.CurrentPlayer);
    }

    [Fact]
    public void Man_PromotesToKingOnBackRank()
    {
        var game = new CheckersGame();
        game.LoadPosition(
            new[]
            {
                (new Position(6, 1), new Piece(PieceColor.White, PieceKind.Man)),
                (new Position(0, 1), new Piece(PieceColor.Black, PieceKind.Man))
            },
            PieceColor.White);

        var move = game.GetLegalMoves(new Position(6, 1)).Single(candidate => candidate.To == new Position(7, 2));
        Assert.True(game.ApplyMove(move));

        var promoted = game.Board.GetPiece(new Position(7, 2));
        Assert.True(promoted.HasValue);
        Assert.Equal(PieceKind.King, promoted.Value.Kind);
    }

    [Fact]
    public void King_CaptureAllowsMultipleLandingSquares()
    {
        var game = new CheckersGame();
        game.LoadPosition(
            new[]
            {
                (new Position(2, 1), new Piece(PieceColor.White, PieceKind.King)),
                (new Position(3, 2), new Piece(PieceColor.Black, PieceKind.Man)),
                (new Position(0, 1), new Piece(PieceColor.Black, PieceKind.Man))
            },
            PieceColor.White);

        var moves = game.GetLegalMoves(new Position(2, 1));

        Assert.Equal(4, moves.Count);
        Assert.All(moves, move => Assert.True(move.IsCapture));
        Assert.Contains(moves, move => move.To == new Position(4, 3));
        Assert.Contains(moves, move => move.To == new Position(5, 4));
        Assert.Contains(moves, move => move.To == new Position(6, 5));
        Assert.Contains(moves, move => move.To == new Position(7, 6));
    }
}
