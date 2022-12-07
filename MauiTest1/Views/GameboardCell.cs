using System.ComponentModel;
//using static MauiTest1.OptionsPopupCell;
using MauiTest1.Helpers;

namespace MauiTest1
{
    public class GameboardCell : AbsoluteLayout
    {
        protected Button thisButton;
        protected DiagonalBlockShape thisFrame;
        protected Style buttonStyle;
        protected Style pressedButtonStyle;
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

            buttonStyle = (Style)ResourceHelper.FindResource(this, "gameboardCellButton");
            pressedButtonStyle = (Style)ResourceHelper.FindResource(this, "pressedGameboardCellButton");

            HeightRequest = 16;
            WidthRequest = 16;

            GenerateCell();
            PropertyChanged += SetCellStyle;

            MessagingCenter.Subscribe<GameStateViewModel>(this, "LockBoard", (sender) => { LockCell(); });
        }

        public static readonly BindableProperty CellStyleProperty =
            BindableProperty.Create(nameof(CellStyle), typeof(string), typeof(MainPage), "");

        public string CellStyle
        {
            get { return (string)GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
        }

        protected void LockCell()
        {
            thisButton.IsEnabled = false;
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
            DiagonalBlockShape frame = new (3, 3, "WhiteBrush", "Gray3Brush");
            Button button = new();
            button.Style = buttonStyle;

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
                BoardWidth = xBoardSize,
                BoardHeight = yBoardSize,
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
            thisButton.Style = pressedButtonStyle;

            AbsoluteLayout.SetLayoutBounds(thisButton, new Rect(0, 0, 16, 16));

            thisFrame.IsVisible = false;
            MessagingCenter.Send<GameboardCell, int>(this, "SmileyFace", 1);
        }

        internal void ResetCellStatus()
        {
            thisButton.Style = buttonStyle;
            AbsoluteLayout.SetLayoutBounds(thisButton, new Rect(1, 1, 14, 14));

            thisFrame.IsVisible = true;
        }
    }
}
