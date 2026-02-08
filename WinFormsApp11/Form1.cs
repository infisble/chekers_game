using WinFormsApp11.Application;
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
    private readonly CheckersSession _session;

    public Form1()
    {
        InitializeComponent();

        _boardButtons = LoadBoardButtons();
        ConfigureBoardButtons();

        _session = new CheckersSession();
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

        _session.ClickCell(Position.FromIndex(index));
        Render();
    }

    private void Render()
    {
        var presentation = _session.BuildPresentation();
        label1.Text = presentation.StatusText;
        RenderBoard(presentation);
    }

    private void RenderBoard(BoardPresentation presentation)
    {
        foreach (var cell in presentation.Cells)
        {
            var button = _boardButtons[cell.Position.ToIndex()];

            button.BackColor = cell.IsDarkSquare ? DarkSquareColor : LightSquareColor;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = cell.IsDarkSquare
                ? Color.FromArgb(81, 52, 29)
                : Color.FromArgb(208, 191, 150);

            button.ForeColor = Color.Black;
            button.Image = null;
            button.Text = string.Empty;

            if (cell.Piece.HasValue)
            {
                button.Image = cell.Piece.Value.Color == PieceColor.White
                    ? Properties.Resources.white
                    : Properties.Resources.black;

                if (cell.Piece.Value.Kind == PieceKind.King)
                {
                    button.Text = "K";
                    button.ForeColor = cell.Piece.Value.Color == PieceColor.White ? Color.Black : Color.White;
                }
            }

            if (cell.IsSelectable)
            {
                button.FlatAppearance.BorderSize = 3;
                button.FlatAppearance.BorderColor = SelectableBorderColor;
            }

            if (cell.IsSelected)
            {
                button.FlatAppearance.BorderSize = 4;
                button.FlatAppearance.BorderColor = SelectedBorderColor;
            }

            if (cell.IsTarget)
            {
                button.BackColor = TargetSquareColor;
                button.FlatAppearance.BorderSize = 3;
                button.FlatAppearance.BorderColor = SelectedBorderColor;
            }
        }
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
