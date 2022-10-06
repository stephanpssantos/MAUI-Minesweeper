using static MauiTest1.OptionsPopupCell;

namespace MauiTest1
{
    public class OptionsPopupCell : GameboardCell
    {
        private string actionName;
        public new int xPosition;
        public new int yPosition;

        public OptionsPopupCell()
        {
        }

        public OptionsPopupCell(string actionName)
        {
            this.actionName = actionName;

            HeightRequest = 16;
            WidthRequest = 16;

            GenerateCell();
            PropertyChanged += SetCellStyle;
        }

        protected override void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 16;
            button.WidthRequest = 16;
            button.Padding = new Thickness(3, 3, 1, 1);
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));

            frame.IsVisible = false;

            SelectedPopupCellOptions selectedOptions = new() { 
                ActionName = actionName,
                XPosition = xPosition,
                YPosition = yPosition
            };

            MessagingCenter.Send<OptionsPopupCell, SelectedPopupCellOptions>(this, "OptionCellClicked", selectedOptions);
        }

        protected override void OnCellRelease(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 12;
            button.WidthRequest = 12;
            button.Padding = 0;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(2, 2, 12, 12));

            frame.IsVisible = true;
        }
    }

    public class SelectedPopupCellOptions
    {
        public string ActionName { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}
