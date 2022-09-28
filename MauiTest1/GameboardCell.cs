namespace MauiTest1
{
    public class GameboardCell : AbsoluteLayout
    {
        protected int xPosition = 0;
        protected int yPosition = 0;
        protected int xBoardSize = 0;
        protected int yBoardSize = 0;

        public GameboardCell()
        {
            HeightRequest = 16;
            WidthRequest = 16;

            GenerateCell();
        }

        public GameboardCell(int positionX, int positionY, int xBoardSize, int yBoardSize) : this()
        {
            this.xPosition = positionX;
            this.yPosition = positionY;
            this.xBoardSize = xBoardSize;
            this.yBoardSize = yBoardSize;
        }

        protected void GenerateCell()
        {
            DiagonalBlockShape frame = new(3, 3, "#FFFFFF", "#808080");
            Button button = new();
            button.CornerRadius = 0;
            button.HeightRequest = 12;
            button.WidthRequest = 12;
            button.BackgroundColor = Color.FromArgb("#C0C0C0");

            button.Pressed += OnCellPress;
            button.Released += OnCellRelease;

            Children.Add(frame);
            Children.Add(button);

            AbsoluteLayout.SetLayoutBounds(frame, new Rect(0, 0, 16, 16));
            AbsoluteLayout.SetLayoutBounds(button, new Rect(2, 2, 12, 12));
        }

        protected virtual void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 16;
            button.WidthRequest = 16;
            button.BackgroundColor = Color.FromArgb("#808080");
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));

            frame.IsVisible = false;

            if (this.Parent is not Grid cellParent) return;

            GameboardCellOptions options = new() { 
                XBoardSize = xBoardSize,
                YBoardSize = yBoardSize,
                XPosition = xPosition,
                YPosition = yPosition, 
                ParentX = (int)cellParent.X, 
                ParentY = (int)cellParent.Y
            };

            MessagingCenter.Send<GameboardCell, GameboardCellOptions>(this, "CellClick", options);
        }

        protected virtual void OnCellRelease(object sender, EventArgs e)
        {
            //if (Children.Count < 2) return;
            //if (sender is not Button button) return;
            //if (Children[0] is not DiagonalBlockShape frame) return;

            //button.HeightRequest = 12;
            //button.WidthRequest = 12;
            //AbsoluteLayout.SetLayoutBounds(button, new Rect(2, 2, 12, 12));

            //frame.IsVisible = true;
        }
    }

    public class GameboardCellOptions
    {
        public GameboardCellOptions()
        {
        }

        public int XBoardSize { get; set; }
        public int YBoardSize { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int ParentX { get; set; }
        public int ParentY { get; set; }
    }
}
