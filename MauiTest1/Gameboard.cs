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
        }

        public static readonly BindableProperty BoardHeightProperty =
            BindableProperty.Create(nameof(BoardHeight), typeof(int), typeof(Gameboard), 20);
        public static readonly BindableProperty BoardWidthProperty =
             BindableProperty.Create(nameof(BoardWidth), typeof(int), typeof(Gameboard), 20);

        public int BoardHeight
        {
            get { return (int)GetValue(BoardHeightProperty); }
            set { SetValue(BoardHeightProperty, value); }
        }
        public int BoardWidth
        {
            get { return (int)GetValue(BoardWidthProperty); }
            set { SetValue(BoardWidthProperty, value); }
        }

        private void GenerateBoard()
        {
            for (int i = 0; i < BoardHeight; i++)
            {
                RowDefinitions.Add(new RowDefinition { Height = new GridLength(16) });
            }
            for (int i = 0; i < BoardWidth; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(16) });
            }

            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
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
