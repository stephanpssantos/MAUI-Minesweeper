using System.ComponentModel;
using static MauiTest1.OptionsPopupCell;

namespace MauiTest1
{
    public class GameboardCell : AbsoluteLayout
    {
        private Button thisButton;
        private DiagonalBlockShape thisFrame;
        protected int internalValue = 0;
        protected int xPosition = 0;
        protected int yPosition = 0;
        protected int xBoardSize = 0;
        protected int yBoardSize = 0;

        public GameboardCell()
        {
        }

        public GameboardCell(int positionX, int positionY, int xBoardSize, int yBoardSize, int internalValue)
        {
            this.xPosition = positionX;
            this.yPosition = positionY;
            this.xBoardSize = xBoardSize;
            this.yBoardSize = yBoardSize;
            this.internalValue = internalValue;

            HeightRequest = 16;
            WidthRequest = 16;

            GenerateCell();
            PropertyChanged += SetCellStyle;
        }

        public static readonly BindableProperty CellStyleProperty =
            BindableProperty.Create(nameof(CellStyle), typeof(string), typeof(MainPage), "");

        public string CellStyle
        {
            get { return (string)GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
        }

        protected void SetCellStyle(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "CellStyle") return;

            switch (CellStyle)
            {
                case "Blank":
                    thisButton.ImageSource = "";
                    ResetCellStatus();
                    break;
                case "Mark":
                    thisButton.ImageSource = "question.png";
                    ResetCellStatus();
                    break;
                case "Flag":
                    thisButton.ImageSource = "flag.png";
                    ResetCellStatus();
                    break;
                case "Clear": // For OptionsPopupCell
                    thisButton.ImageSource = "clear_mark.png";
                    ResetCellStatus();
                    break;
                case "Cancel": // For OptionsPopupCell
                    thisButton.ImageSource = "cancel_mark.png";
                    ResetCellStatus();
                    break;
                case "Cleared":
                    SetPressedStatus();
                    thisButton.ImageSource = "";
                    if (internalValue > 0) thisButton.Text = internalValue.ToString();
                    thisButton.Pressed -= OnCellPress;
                    break;
                case "Exploded":
                    SetPressedStatus();
                    thisButton.ImageSource = "mine.png";
                    //thisButton.Pressed -= OnCellPress; // The containing grid is disabled instead
                    break;
                case "ExplosionSite":
                    SetPressedStatus();
                    thisButton.BackgroundColor = Colors.Red;
                    thisButton.ImageSource = "exploded_mine.png";
                    //thisButton.Pressed -= OnCellPress;
                    break;
                case "FalseFlag":
                    thisButton.ImageSource = "not_a_mine.png";
                    ResetCellStatus();
                    break;
                case "Reset":
                    // Do nothing
                    break;
                default:
                    break;
            }
        }

        protected virtual void GenerateCell()
        {
            DiagonalBlockShape frame = new(3, 3, "#FFFFFF", "#808080");
            Button button = new();
            button.CornerRadius = 0;
            button.HeightRequest = 14;
            button.WidthRequest = 14;
            button.Padding = 1;
            button.BackgroundColor = Color.FromArgb("#C0C0C0");
            button.FontFamily = "8Bit";
            button.FontSize = 12;
            button.ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0);

            switch (internalValue)
            {
                case 1:
                    button.TextColor = Colors.Blue;
                    break;
                case 2:
                    button.TextColor = Colors.Green;
                    break;
                case 3:
                    button.TextColor = Colors.Red;
                    break;
                case 4:
                    button.TextColor = Colors.DarkBlue;
                    break;
                case 5:
                    button.TextColor = Colors.Maroon;
                    break;
                case 6:
                    button.TextColor = Colors.DarkViolet;
                    break;
                case 7:
                    button.TextColor = Colors.Brown;
                    break;
                case 8:
                    button.TextColor = Colors.Gold;
                    break;
                default:
                    break;
            }

            button.Pressed += OnCellPress;
            button.Released += OnCellRelease;

            AbsoluteLayout.SetLayoutBounds(frame, new Rect(0, 0, 16, 16));
            AbsoluteLayout.SetLayoutBounds(button, new Rect(1, 1, 14, 14));

            Children.Add(frame);
            Children.Add(button);

            thisFrame = frame;
            thisButton = button;
        }

        protected virtual void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (this.Parent is not Grid cellParent) return;

            SetPressedStatus();

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
        }

        protected void SetPressedStatus()
        {
            thisButton.HeightRequest = 16;
            thisButton.WidthRequest = 16;
            thisButton.Padding = 2;

            //thisButton.BackgroundColor = Color.FromArgb("#808080");

            AbsoluteLayout.SetLayoutBounds(thisButton, new Rect(0, 0, 16, 16));

            thisFrame.IsVisible = false;
            MessagingCenter.Send<GameboardCell, int>(this, "GameButtonShockFace", 1);
        }

        internal void ResetCellStatus(int toggleSmiley = 0)
        {
            if (toggleSmiley == 0)
            {
                // GameboardOptionsPopup.xaml
                MessagingCenter.Send<GameboardCell, int>(this, "GameButtonShockFace", 0);
            }
            thisButton.BackgroundColor = Color.FromArgb("#C0C0C0");
            thisButton.HeightRequest = 14;
            thisButton.WidthRequest = 14;
            thisButton.Padding = 1;
            AbsoluteLayout.SetLayoutBounds(thisButton, new Rect(1, 1, 14, 14));

            thisFrame.IsVisible = true;
        }
    }

    public class GameboardCellOptions
    {
        public int XBoardSize { get; set; }
        public int YBoardSize { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int ParentX { get; set; }
        public int ParentY { get; set; }
    }
}
