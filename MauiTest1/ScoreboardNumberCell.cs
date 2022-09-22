using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel; // PropertyChangedEventArgs
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace MauiTest1
{
    public class ScoreboardNumberCell : AbsoluteLayout
    {
        public enum LineOrientation { Horizontal, Vertical };
        public enum CellOrientation { Top, Bottom, Left, Right, Center };
        public enum CellState { RedEnabled, RedDisabled };

        public ScoreboardNumberCell()
        {
            CreateCell();
            PropertyChanged += SetCellColor;
        }

        // This is intentionally not calling the parameterless constructor.
        // It will not render correctly if orientation is set after CreateCell is called.
        public ScoreboardNumberCell(CellOrientation orientation, CellState state)
        {
            ThisOrientation = orientation;
            ThisState = state;

            CreateCell();
            PropertyChanged += SetCellColor;
        }

        public static readonly BindableProperty ThisStateProperty =
            BindableProperty.Create(nameof(ThisState), typeof(CellState), typeof(ScoreboardNumberCell), CellState.RedDisabled);
        public static readonly BindableProperty ThisOrientationProperty =
            BindableProperty.Create(nameof(ThisOrientation), typeof(CellOrientation), typeof(ScoreboardNumberCell), CellOrientation.Top);

        public CellOrientation ThisOrientation
        {
            get { return (CellOrientation)GetValue(ThisOrientationProperty); }
            set { SetValue(ThisOrientationProperty, value); }
        }
        public CellState ThisState
        {
            get { return (CellState)GetValue(ThisStateProperty); }
            set { SetValue(ThisStateProperty, value); }
        }

        private void SetCellColor(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;

            if (args.PropertyName != "ThisState") return;

            if (Children.Count != 6) return;
            
            string rowAColor;
            string rowBColor;

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

            Path line1_1 = Children[0] as Path;
            Path line1_2 = Children[1] as Path;
            Path line2_1 = Children[2] as Path;
            Path line2_2 = Children[3] as Path;
            Path line3_1 = Children[4] as Path;
            Path line3_2 = Children[5] as Path;

            line1_1.Fill = Color.FromArgb(rowAColor);
            line1_2.Fill = Color.FromArgb(rowBColor);
            line2_1.Fill = Color.FromArgb(rowAColor);
            line2_2.Fill = Color.FromArgb(rowBColor);
            line3_1.Fill = Color.FromArgb(rowAColor);
            line3_2.Fill = Color.FromArgb(rowBColor);
        }

        private void CreateCell()
        {
            LineOrientation orientation = LineOrientation.Horizontal;

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
                line1_1 = CreateLine(5, 2, null, orientation);
                line1_2 = CreateLine(3, 3, null, orientation);
            }
            else
            {
                line1_1 = CreateLine(9, 0, null, orientation);
                line1_2 = CreateLine(7, 1, null, orientation);
            }
            Path line2_1 = CreateLine(7, 1, null, orientation);
            Path line2_2 = CreateLine(5, 2, null, orientation);
            Path line3_1 = CreateLine(5, 2, null, orientation);
            Path line3_2 = CreateLine(3, 3, null, orientation);

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

            SetCellColor(this, new PropertyChangedEventArgs("ThisState"));

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
