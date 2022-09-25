namespace MauiTest1
{
    public class GameboardCell : AbsoluteLayout
    {
        public GameboardCell()
        {
            HeightRequest = 16;
            WidthRequest = 16;

            GenerateCell();
        }

        private void GenerateCell()
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

        private void OnCellPress(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;
            if (sender is not Button button) return;
            if (Children[0] is not DiagonalBlockShape frame) return;

            button.HeightRequest = 16;
            button.WidthRequest = 16;
            AbsoluteLayout.SetLayoutBounds(button, new Rect(0, 0, 16, 16));

            frame.IsVisible = false;
        }

        private void OnCellRelease(object sender, EventArgs e)
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
