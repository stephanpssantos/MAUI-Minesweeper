using System.ComponentModel;

namespace MauiTest1
{
    public class Gameboard : Grid
    {
        public Gameboard()
        {
            BackgroundColor = Color.FromArgb("#808080");
            ColumnSpacing = 0;
            RowSpacing = 0;
            GenerateBoard();

            PropertyChanged += RegenerateBoard;
            PropertyChanged += UpdateBoardState;
        }

        public static readonly BindableProperty BoardSetupProperty =
            BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(Gameboard), GameboardSetupFactory.NewBeginnerSetup());

        public static readonly BindableProperty BoardStateProperty =
            BindableProperty.Create(nameof(BoardState), typeof(int[,]), typeof(Gameboard), new int[8, 8]);

        public GameboardSetup BoardSetup
        {
            get { return (GameboardSetup)GetValue(BoardSetupProperty); }
            set { SetValue(BoardSetupProperty, value); }
        }

        public int[,] BoardState 
        { 
            get { return (int[,])GetValue(BoardStateProperty); }
            set { SetValue(BoardStateProperty, value); }
        }

        private void RegenerateBoard(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "BoardSetup") return;
            if (Children.Count > 0)
            {
                RowDefinitions.Clear();
                ColumnDefinitions.Clear();
                Children.Clear();
            }

            GenerateBoard();
        }

        private void UpdateBoardState(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "BoardState") return;

            for (int i = 0; i < BoardSetup.BoardHeight; i++)
            {
                for (int j = 0; j < BoardSetup.BoardWidth; j++)
                {
                    if (Children[(i * BoardSetup.BoardWidth) + j] is not GameboardCell cell) return;
                    int boardStateCode = BoardState[j, i];
                    switch(boardStateCode)
                    {
                        case 0: // closed
                            cell.CellStyle = "Blank";
                            break;
                        case 1: // question mark
                            cell.CellStyle = "Mark";
                            break;
                        case 2:
                            cell.CellStyle = "Flag";
                            break;
                        case 3: // Opened
                            cell.CellStyle = "Cleared";
                            break;
                        case 4:
                            cell.CellStyle = "Exploded";
                            break;
                        case 5:
                            cell.CellStyle = "ExplosionSite";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void GenerateBoard()
        {
            for (int i = 0; i < BoardSetup.BoardHeight; i++)
            {
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(16) });
            }
            for (int i = 0; i < BoardSetup.BoardWidth; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(16) });
            }

            for (int i = 0; i < BoardSetup.BoardHeight; i++)
            {
                for (int j = 0; j < BoardSetup.BoardWidth; j++)
                {
                    GameboardCell cell = new(j, i, BoardSetup.BoardWidth, BoardSetup.BoardHeight, BoardSetup.BoardPositions[j, i]);
                    Add(cell);
                    SetRow(cell as IView, i);
                    SetColumn(cell as IView, j);
                }
            }

            ResetBoardState();
        }

        private void ResetBoardState()
        {
            BoardState = new int[BoardSetup.BoardWidth, BoardSetup.BoardHeight];
        }
    }
}
