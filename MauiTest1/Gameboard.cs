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
        }

        public static readonly BindableProperty BoardSetupProperty =
            BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(Gameboard), GameboardSetupFactory.NewBeginnerSetup());

        public GameboardSetup BoardSetup
        {
            get { return (GameboardSetup)GetValue(BoardSetupProperty); }
            set { SetValue(BoardSetupProperty, value); }
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
                    GameboardCell cell = new();
                    Add(cell);
                    SetRow(cell as IView, i);
                    SetColumn(cell as IView, j);
                }
            }
        }
    }
}
