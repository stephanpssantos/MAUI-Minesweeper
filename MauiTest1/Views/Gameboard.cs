using System.Collections.ObjectModel;
using System.ComponentModel;
using MauiTest1.Helpers;

namespace MauiTest1
{
    public class Gameboard : Grid
    {
        public Gameboard()
        {
            BackgroundColor = (Color)ResourceHelper.FindResource(this, "Gray3"); ;
            ColumnSpacing = 0;
            RowSpacing = 0;
            GenerateBoard();

            PropertyChanged += RegenerateBoard;
        }

        public static readonly BindableProperty BoardSetupProperty =
            BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(Gameboard), GameboardSetupFactory.NewBeginnerSetup());

        public static readonly BindableProperty BoardStateProperty =
            BindableProperty.Create(nameof(BoardState), typeof(ObservableCollection<int>), typeof(Gameboard), null);

        public GameboardSetup BoardSetup
        {
            get { return (GameboardSetup)GetValue(BoardSetupProperty); }
            set { SetValue(BoardSetupProperty, value); }
        }

        public ObservableCollection<int> BoardState
        {
            get { return (ObservableCollection<int>)GetValue(BoardStateProperty); }
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
            //if (BoardState is null || BoardState.Count == 0) return;
            //if (e is not PropertyChangedEventArgs args) return;
            //if (args.PropertyName != "BoardState") return;

            for (int i = 0; i < BoardSetup.BoardHeight; i++)
            {
                for (int j = 0; j < BoardSetup.BoardWidth; j++)
                {
                    if (Children[(i * BoardSetup.BoardWidth) + j] is not GameboardCell cell) return;
                    int boardStateCode = BoardState[(i * BoardSetup.BoardWidth) + j];
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
                        case 6:
                            cell.CellStyle = "FalseFlag";
                            break;
                        // Same properties as closed. Used to retrigger 'closed' property changed.
                        case 7:
                            cell.CellStyle = "Reset";
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

            BoardState = ResetBoardState(BoardSetup.BoardWidth, BoardSetup.BoardHeight);
            BoardState.CollectionChanged += UpdateBoardState;
        }

        private static ObservableCollection<int> ResetBoardState(int width, int height)
        {
            ObservableCollection<int> boardState = new();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boardState.Add(0);
                }
            }

            return boardState;
        }
    }
}
