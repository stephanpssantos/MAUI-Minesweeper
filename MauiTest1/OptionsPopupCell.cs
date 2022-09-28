namespace MauiTest1
{
    public class OptionsPopupCell : GameboardCell
    {
        public OptionsPopupCell() : base()
        {
        }


        protected override void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 16;
            button.WidthRequest = 16;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));

            frame.IsVisible = false;
        }

        protected override void OnCellRelease(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 12;
            button.WidthRequest = 12;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(2, 2, 12, 12));

            frame.IsVisible = true;
        }
    }
}
