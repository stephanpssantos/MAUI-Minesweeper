using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace MauiTest1
{
    public class DiagonalBlockShape : AbsoluteLayout
    {
        private int BorderDepth = 5;
        public DiagonalBlockShape()
        {
            Loaded += ResizeShape;
            SizeChanged += ResizeShape;
        }

        public DiagonalBlockShape(int borderDepth) : this()
        {
            BorderDepth = borderDepth;
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
                    new Point(ThisObject.Width - BorderDepth, BorderDepth),
                    new Point(BorderDepth, BorderDepth),
                    new Point(BorderDepth, ThisObject.Height - BorderDepth),
                    new Point(0, ThisObject.Height)
                },
                Fill = new SolidColorBrush(Color.FromArgb("#808080"))
            };

            Polygon shape2 = new()
            {
                Points = new PointCollection()
                {
                    new Point(ThisObject.Width, 0),
                    new Point(ThisObject.Width - BorderDepth, BorderDepth),
                    new Point(ThisObject.Width - BorderDepth, ThisObject.Height - BorderDepth),
                    new Point(BorderDepth, ThisObject.Height - BorderDepth),
                    new Point(0, ThisObject.Height),
                    new Point(ThisObject.Width, ThisObject.Height)
                },
                Fill = new SolidColorBrush(Color.FromArgb("#FFFFFF"))
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
