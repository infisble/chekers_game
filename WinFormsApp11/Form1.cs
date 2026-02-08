using WinFormsApp11.Game;

namespace WinFormsApp11;

public partial class Form1 : Form
{
    private static readonly Color LightSquareColor = Color.FromArgb(242, 229, 194);
    private static readonly Color DarkSquareColor = Color.FromArgb(116, 78, 49);
    private static readonly Color SelectableBorderColor = Color.FromArgb(80, 153, 76);
    private static readonly Color SelectedBorderColor = Color.FromArgb(37, 99, 235);
    private static readonly Color TargetSquareColor = Color.FromArgb(245, 208, 66);

    private readonly Button[] _boardButtons;
    private readonly CheckersGame _game;

    private Position? _selectedPosition;
    private List<Move> _selectedMoves = new();

    public Form1()
    {
        InitializeComponent();

        _boardButtons = LoadBoardButtons();
        ConfigureBoardButtons();

        _game = new CheckersGame();
        SetStatus($"Ход: {ColorText(_game.CurrentPlayer)}");
        Render();
    }

    private Button[] LoadBoardButtons()
    {
        var buttons = new Button[Position.BoardSize * Position.BoardSize];

        for (var index = 0; index < buttons.Length; index++)
        {
            var button = Controls.Find($"button{index}", true).FirstOrDefault() as Button;
            if (button is null)
            {
                throw new InvalidOperationException($"Board button button{index} was not found in the form.");
            }

            buttons[index] = button;
        }

        return buttons;
    }

    private void ConfigureBoardButtons()
    {
        for (var index = 0; index < _boardButtons.Length; index++)
        {
            var button = _boardButtons[index];
            button.Tag = index;
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Click += BoardCell_Click;
        }
    }

    private void BoardCell_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.Tag is not int index)
        {
            return;
        }

        HandleBoardClick(Position.FromIndex(index));
    }

    private void HandleBoardClick(Position position)
    {
        if (_game.IsFinished)
        {
            return;
        }

        var selectedMove = _selectedMoves.FirstOrDefault(move => move.To == position);
        if (_selectedPosition.HasValue && selectedMove is not null)
        {
            if (_game.ApplyMove(selectedMove))
            {
                if (_game.IsFinished)
                {
                    ClearSelection();
                    SetStatus($"Победа: {ColorText(_game.Winner!.Value)}");
                }
                else if (_game.ForcedCapturePiece.HasValue)
                {
                    SelectPiece(_game.ForcedCapturePiece.Value);
                    SetStatus($"Продолжите взятие: {ColorText(_game.CurrentPlayer)}");
                }
                else
                {
                    ClearSelection();
                    SetStatus($"Ход: {ColorText(_game.CurrentPlayer)}");
                }
            }

            Render();
            return;
        }

        var piece = _game.Board.GetPiece(position);
        var selectable = _game.GetSelectablePieces();
        if (piece.HasValue && piece.Value.Color == _game.CurrentPlayer && selectable.Contains(position))
        {
            SelectPiece(position);

            var captureOnly = _selectedMoves.Any(static move => move.IsCapture);
            SetStatus(captureOnly
                ? $"Выбрана фигура: обязательное взятие ({_selectedMoves.Count} вариантов)"
                : $"Выбрана фигура: доступно ходов {_selectedMoves.Count}");
        }
        else
        {
            ClearSelection();
            SetStatus($"Ход: {ColorText(_game.CurrentPlayer)}");
        }

        Render();
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

    private void Render()
    {
        RenderBoard();
        HighlightLegalState();
    }

    private void RenderBoard()
    {
        for (var row = 0; row < Position.BoardSize; row++)
        {
            for (var column = 0; column < Position.BoardSize; column++)
            {
                var position = new Position(row, column);
                var button = _boardButtons[position.ToIndex()];

                var isDarkSquare = (row + column) % 2 == 1;
                button.BackColor = isDarkSquare ? DarkSquareColor : LightSquareColor;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = isDarkSquare
                    ? Color.FromArgb(81, 52, 29)
                    : Color.FromArgb(208, 191, 150);

                button.ForeColor = Color.Black;
                button.Image = null;
                button.Text = string.Empty;

                var piece = _game.Board.GetPiece(position);
                if (!piece.HasValue)
                {
                    continue;
                }

                button.Image = piece.Value.Color == PieceColor.White
                    ? Properties.Resources.white
                    : Properties.Resources.black;

                if (piece.Value.Kind == PieceKind.King)
                {
                    button.Text = "K";
                    button.ForeColor = piece.Value.Color == PieceColor.White ? Color.Black : Color.White;
                }
            }
        }
    }

    private void HighlightLegalState()
    {
        foreach (var position in _game.GetSelectablePieces())
        {
            var button = _boardButtons[position.ToIndex()];
            button.FlatAppearance.BorderSize = 3;
            button.FlatAppearance.BorderColor = SelectableBorderColor;
        }

        if (!_selectedPosition.HasValue)
        {
            return;
        }

        var selectedButton = _boardButtons[_selectedPosition.Value.ToIndex()];
        selectedButton.FlatAppearance.BorderSize = 4;
        selectedButton.FlatAppearance.BorderColor = SelectedBorderColor;

        foreach (var move in _selectedMoves)
        {
            var targetButton = _boardButtons[move.To.ToIndex()];
            targetButton.BackColor = TargetSquareColor;
            targetButton.FlatAppearance.BorderSize = 3;
            targetButton.FlatAppearance.BorderColor = SelectedBorderColor;
        }
    }

    private void SetStatus(string text)
    {
        label1.Text = text;
    }

    private static string ColorText(PieceColor color)
    {
        return color == PieceColor.White ? "белые" : "черные";
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void button10_Click(object sender, EventArgs e)
    {
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void button48_Click(object sender, EventArgs e)
    {
    }

    private void button49_Click(object sender, EventArgs e)
    {
    }

    private void button40_Click(object sender, EventArgs e)
    {
    }

    private void button15_Click(object sender, EventArgs e)
    {
    }

    private void button12_Click(object sender, EventArgs e)
    {
    }
}
