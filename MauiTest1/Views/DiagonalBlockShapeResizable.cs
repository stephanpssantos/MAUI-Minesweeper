using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using MauiTest1.Helpers;
using System.ComponentModel;

namespace MauiTest1
{
    /// <summary>
    /// Binds to a BoardSetupProperty property to determine width and height.
    /// </summary>
    public class DiagonalBlockShapeResizable : DiagonalBlockShape
    {
        private int topBorderDepth = 5;
        private int bottomBorderDepth = 5;

        public DiagonalBlockShapeResizable() : base()
        {
            PropertyChanged += ResizeWhole;
        }

        public DiagonalBlockShapeResizable(int borderDepth) : this()
        {
            this.topBorderDepth = borderDepth;
            this.bottomBorderDepth = borderDepth;
        }

        public static readonly BindableProperty BoardSetupProperty =
            BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(GameboardGraphicsView), GameboardSetupFactory.NewBeginnerSetup());

        public GameboardSetup BoardSetup
        {
            get { return (GameboardSetup)GetValue(BoardSetupProperty); }
            set { SetValue(BoardSetupProperty, value); }
        }

        private void ResizeWhole(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;
            if (args.PropertyName != "BoardSetup") return;

            this.WidthRequest = (BoardSetup.BoardWidth * 16) + topBorderDepth + bottomBorderDepth;
            this.HeightRequest = (BoardSetup.BoardHeight * 16) + topBorderDepth + bottomBorderDepth;
        }
    }
}
