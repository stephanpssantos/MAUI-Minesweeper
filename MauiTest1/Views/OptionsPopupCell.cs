using MauiTest1.Helpers; // ResourceHelper

namespace MauiTest1
{
    public class OptionsPopupCell : AbsoluteLayout
    {
        private Style buttonStyle;
        private Style pressedButtonStyle;
        private string actionName;

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
        }

        public int XPosition { get; set; }
        public int YPosition { get; set; }

        private void GenerateCell()
        {
            DiagonalBlockShape frame = new(3, 3, "WhiteBrush", "Gray3Brush");
            Button button = new();
            button.Style = buttonStyle;

            button.ImageSource = actionName switch
            {
                "Mark" => "question.png",
                "Flag" => "flag.png",
                "Clear" => "clear_mark.png",
                "Cancel" => "cancel_mark.png",
                _ => "cancel_mark.png"
            };

            button.Pressed += OnCellPress;
            button.Released += OnCellRelease;

            AbsoluteLayout.SetLayoutBounds(frame, new Rect(0, 0, 16, 16));
            AbsoluteLayout.SetLayoutBounds(button, new Rect(1, 1, 14, 14));

            Children.Add(frame);
            Children.Add(button);
        }

        private void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.Style = pressedButtonStyle;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));
            
            frame.IsVisible = false;
        }

        private void OnCellRelease(object sender, EventArgs e)
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
                XPosition = this.XPosition,
                YPosition = this.YPosition
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
