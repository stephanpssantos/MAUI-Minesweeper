using Microsoft.Maui.Controls.Shapes;

namespace MauiTest1
{
    public class DiagonalBlockShape : AbsoluteLayout
    {
        private int BorderDepth = 3;
        public DiagonalBlockShape()
        {
            Loaded += ResizeShape;
            SizeChanged += ResizeShape;
        }

        private void ResizeShape(object sender, EventArgs e)
        {
            AbsoluteLayout ThisObject = sender as AbsoluteLayout;

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

            if (ThisObject.Children.Count == 2)
            {
                ThisObject.Children[0] = shape1;
                ThisObject.Children[1] = shape2;
            }
            else if (ThisObject.Children.Count == 0)
            {
                ThisObject.Children.Add(shape1);
                ThisObject.Children.Add(shape2);
            }

            AbsoluteLayout.SetLayoutFlags(shape1, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(shape1, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(shape2, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(shape2, new Rect(0, 0, 1, 1));
        }
    }
}
