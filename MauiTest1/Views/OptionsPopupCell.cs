using MauiTest1.Helpers;

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

            buttonStyle = (Style)ResourceHelper.FindResource(this, "gameboardCellButton");
            pressedButtonStyle = (Style)ResourceHelper.FindResource(this, "pressedOptionsGameboardCellButton");

            GenerateCell();
            PropertyChanged += SetCellStyle;
        }

        protected override void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.Style = pressedButtonStyle;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));
            
            frame.IsVisible = false;
        }

        protected override void OnCellRelease(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.Style = buttonStyle;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(1, 1, 14, 14));

            frame.IsVisible = true;

            SelectedPopupCellOptions selectedOptions = new()
            {
                ActionName = actionName,
                XPosition = xPosition,
                YPosition = yPosition
            };

            MessagingCenter.Send<OptionsPopupCell, SelectedPopupCellOptions>(this, "OptionCellClicked", selectedOptions);
            MessagingCenter.Send<OptionsPopupCell>(this, "ClosePopup");
        }
    }

    public class SelectedPopupCellOptions
    {
        public string ActionName { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}
