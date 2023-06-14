using System.ComponentModel;

namespace MauiTest1
{
    public class ImageOverlayWrapper : AbsoluteLayout
    {
        public ImageOverlayWrapper()
        {
            PropertyChanged += BoardSizeChanged;
        }

        public static readonly BindableProperty BoardSetupProperty =
            BindableProperty.Create(
                nameof(BoardSetup),
                typeof(GameboardSetup),
                typeof(ImageOverlayWrapper)
            );

        public GameboardSetup BoardSetup
        {
            get { return (GameboardSetup)GetValue(BoardSetupProperty); }
            set { SetValue(BoardSetupProperty, value); }
        }

        private void BoardSizeChanged(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "BoardSetup") return;
            // This method is specific to this project

            this.WidthRequest = (BoardSetup.BoardWidth * 16);
            this.HeightRequest = (BoardSetup.BoardHeight * 16);
        }
    }
}
