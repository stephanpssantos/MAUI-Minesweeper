using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace MauiTest1
{
    internal class ScoreboardNumberCell : AbsoluteLayout
    {
        public enum LineOrientation { Horizontal, Vertical };
        public enum CellOrientation { Top, Bottom, Left, Right, Center };
        public enum CellState { RedEnabled, RedDisabled };
        public CellOrientation ThisOrientation = CellOrientation.Top;
        public CellState ThisState = CellState.RedDisabled;

        public ScoreboardNumberCell()
        {
        }

        public ScoreboardNumberCell(CellOrientation orientation, CellState state)
        {
            ThisOrientation = orientation;
            ThisState = state;

            Loaded += CreateCell;
            // SizeChanged += CreateCell; // (?)
        }

        private void CreateCell(object sender, EventArgs e)
        {
            LineOrientation orientation = LineOrientation.Horizontal;

            string rowAColor = string.Empty;
            string rowBColor = string.Empty;

            if (ThisState == CellState.RedEnabled)
            {
                rowAColor = "#FFFF0000";
                rowBColor = "#FFFF0000";
            }
            else
            {
                rowAColor = "#FF800000";
                rowBColor = "#FF000000";
            }

            if (ThisOrientation == CellOrientation.Left || ThisOrientation == CellOrientation.Right)
            {
                HeightRequest = 9;
                WidthRequest = 3;
                orientation = LineOrientation.Vertical;
            }
            else
            {
                HeightRequest = 3;
                WidthRequest = 9;
            }

            Path line1_1;
            Path line1_2;
            if (ThisOrientation == CellOrientation.Center)
            {
                line1_1 = CreateLine(5, 2, rowAColor, orientation);
                line1_2 = CreateLine(3, 3, rowBColor, orientation);
            }
            else
            {
                line1_1 = CreateLine(9, 0, rowAColor, orientation);
                line1_2 = CreateLine(7, 1, rowBColor, orientation);
            }
            Path line2_1 = CreateLine(7, 1, rowAColor, orientation);
            Path line2_2 = CreateLine(5, 2, rowBColor, orientation);
            Path line3_1 = CreateLine(5, 2, rowAColor, orientation);
            Path line3_2 = CreateLine(3, 3, rowBColor, orientation);

            Children.Add(line1_1);
            Children.Add(line1_2);
            Children.Add(line2_1);
            Children.Add(line2_2);
            Children.Add(line3_1);
            Children.Add(line3_2);

            int x1 = 0;
            int x2 = 0;
            int x3 = 0;
            int y1 = 0;
            int y2 = 0;
            int y3 = 0;

            if (ThisOrientation == CellOrientation.Top)
            {
                y2 = 1;
                y3 = 2;
            }
            else if (ThisOrientation == CellOrientation.Bottom)
            {
                y1 = 2;
                y2 = 1;
            }
            else if (ThisOrientation == CellOrientation.Left)
            {
                x2 = 1;
                x3 = 2;
            }
            else if (ThisOrientation == CellOrientation.Right)
            {
                x1 = 2;
                x2 = 1;
            }
            else if (ThisOrientation == CellOrientation.Center)
            {
                y2 = 1;
                y3 = 2;
            }

            AbsoluteLayout.SetLayoutBounds(line1_1, new Rect(x1, y1, WidthRequest, HeightRequest));
            AbsoluteLayout.SetLayoutBounds(line1_2, new Rect(x1, y1, WidthRequest, HeightRequest));
            AbsoluteLayout.SetLayoutBounds(line2_1, new Rect(x2, y2, WidthRequest, HeightRequest));
            AbsoluteLayout.SetLayoutBounds(line2_2, new Rect(x2, y2, WidthRequest, HeightRequest));
            AbsoluteLayout.SetLayoutBounds(line3_1, new Rect(x3, y3, WidthRequest, HeightRequest));
            AbsoluteLayout.SetLayoutBounds(line3_2, new Rect(x3, y3, WidthRequest, HeightRequest));

        }

        private Path CreateLine(
            int size = 3,
            int offset = 0,
            string color = "#FFFF0000",
            LineOrientation orientation = LineOrientation.Horizontal)
        {
            int verticalOffset = 0;
            int horizontalOffset = 0;
            int verticalPosition = 0;
            int horizontalPosition = 0;

            GeometryGroup geometryGroup = new();

            if (orientation == LineOrientation.Horizontal)
            {
                horizontalOffset = offset;
            } 
            else
            {
                verticalOffset = offset;
            }

            for (int i = 0; i < size; i += 2)
            {
                if (orientation == LineOrientation.Horizontal)
                {
                    horizontalPosition = i;
                }
                else
                {
                    verticalPosition = i;
                }
                RectangleGeometry newRect = new();
                newRect.Rect = new Rect(horizontalPosition, verticalPosition, 1, 1);
                geometryGroup.Children.Add(newRect);
            }

            Path path = new();
            path.Fill = Color.FromArgb(color);
            path.StrokeThickness = 0;
            path.Margin = new Thickness(horizontalOffset, verticalOffset, 0, 0);

            path.Data = geometryGroup;

            return path;
        }
    }
}
