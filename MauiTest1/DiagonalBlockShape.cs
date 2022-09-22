using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MauiTest1
{
    public class DiagonalBlockShape : AbsoluteLayout
    {
        private int TopBorderDepth = 5;
        private int BottomBorderDepth = 5;
        private string TopColor = "#808080";
        private string BottomColor = "#FFFFFF";

        public DiagonalBlockShape()
        {
            Loaded += ResizeShape;
            SizeChanged += ResizeShape;
        }

        public DiagonalBlockShape(int borderDepth) : this()
        {
            TopBorderDepth = borderDepth;
            BottomBorderDepth = borderDepth;
        }

        public DiagonalBlockShape(int topBorderDepth, int bottomBorderDepth, string topColor, string bottomColor) : this()
        {
            TopBorderDepth = topBorderDepth;
            BottomBorderDepth = bottomBorderDepth;
            TopColor = topColor;
            BottomColor = bottomColor;
        }

        private void ResizeShape(object sender, EventArgs e)
        {
            AbsoluteLayout ThisObject = sender as AbsoluteLayout;

            ThisObject.Children.Clear();

            Polygon shape1 = new()
            {
                Points = new PointCollection()
                {
                    new Point(0, 0),
                    new Point(ThisObject.Width, 0),
                    new Point(ThisObject.Width - TopBorderDepth, TopBorderDepth),
                    new Point(TopBorderDepth, TopBorderDepth),
                    new Point(TopBorderDepth, ThisObject.Height - TopBorderDepth),
                    new Point(0, ThisObject.Height)
                },
                Fill = new SolidColorBrush(Color.FromArgb(TopColor))
            };

            Polygon shape2 = new()
            {
                Points = new PointCollection()
                {
                    new Point(ThisObject.Width, 0),
                    new Point(ThisObject.Width - BottomBorderDepth, BottomBorderDepth),
                    new Point(ThisObject.Width - BottomBorderDepth, ThisObject.Height - BottomBorderDepth),
                    new Point(BottomBorderDepth, ThisObject.Height - BottomBorderDepth),
                    new Point(0, ThisObject.Height),
                    new Point(ThisObject.Width, ThisObject.Height)
                },
                Fill = new SolidColorBrush(Color.FromArgb(BottomColor))
            };

            ThisObject.Children.Add(shape1);
            ThisObject.Children.Add(shape2);

            AbsoluteLayout.SetLayoutFlags(shape1, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutFlags(shape2, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(shape1, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutBounds(shape2, new Rect(0, 0, 1, 1));
        }
    }
}
