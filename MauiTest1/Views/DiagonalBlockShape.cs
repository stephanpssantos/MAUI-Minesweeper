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
            Loaded += MakeShape;
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

        private void MakeShape(object sender, EventArgs e)
        {
            AbsoluteLayout ThisObject = sender as AbsoluteLayout;

            Polygon shape1 = new() { Fill = new SolidColorBrush(Color.FromArgb(TopColor)) };
            Polygon shape2 = new() { Fill = new SolidColorBrush(Color.FromArgb(BottomColor)) };

            ThisObject.Children.Add(shape1);
            ThisObject.Children.Add(shape2);

            AbsoluteLayout.SetLayoutFlags(shape1, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutFlags(shape2, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(shape1, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutBounds(shape2, new Rect(0, 0, 1, 1));

            ResizeShape(this, null);
        }

        private void ResizeShape(object sender, EventArgs e)
        {
            if (Children.Count < 2) return;

            AbsoluteLayout ThisObject = sender as AbsoluteLayout;

            PointCollection new0 = new()
            {
                new Point(0, 0),
                new Point(ThisObject.Width, 0),
                new Point(ThisObject.Width - TopBorderDepth, TopBorderDepth),
                new Point(TopBorderDepth, TopBorderDepth),
                new Point(TopBorderDepth, ThisObject.Height - TopBorderDepth),
                new Point(0, ThisObject.Height)
            };

            PointCollection new1 = new()
            {
                new Point(ThisObject.Width, 0),
                new Point(ThisObject.Width - BottomBorderDepth, BottomBorderDepth),
                new Point(ThisObject.Width - BottomBorderDepth, ThisObject.Height - BottomBorderDepth),
                new Point(BottomBorderDepth, ThisObject.Height - BottomBorderDepth),
                new Point(0, ThisObject.Height),
                new Point(ThisObject.Width, ThisObject.Height)
            };

            Polygon Child0 = Children[0] as Polygon;
            Polygon Child1 = Children[1] as Polygon;

            Child0.Points = new0;
            Child1.Points = new1;
        }
    }
}
