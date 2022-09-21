using Microsoft.Maui.Layouts;

namespace MauiTest1
{
    public class ScoreboardNumber : AbsoluteLayout
    {
        public static readonly BindableProperty NumberProperty =
            BindableProperty.Create(nameof(Number), typeof(int), typeof(ScoreboardNumber), 0);

        //public int Number = 0;
        public ScoreboardNumber()
        {
            BackgroundColor = Colors.Black;
            Padding = 1;
            HeightRequest = 23;
            WidthRequest = 13;

            Loaded += RenderNumber;
            //SizeChanged += RenderNumber; // (?)
        }

        //public ScoreboardNumber(int number) : this()
        //{
        //    Number = number;
        //}

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        private void RenderNumber(object sender, EventArgs e)
        {
            if (Number < 0 || Number > 9)
            {
                return;
            }

            ScoreboardNumberCell.CellState topColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState upperRightColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState lowerRightColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState bottomColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState lowerLeftColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState upperLeftColor = ScoreboardNumberCell.CellState.RedDisabled;
            ScoreboardNumberCell.CellState centerColor = ScoreboardNumberCell.CellState.RedDisabled;

            switch (Number)
            {
                case 0:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 1:
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 2:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 3:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 4:
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 5:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 6:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 7:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 8:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 9:
                default:
                    topColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    lowerRightColor = ScoreboardNumberCell.CellState.RedEnabled;
                    bottomColor = ScoreboardNumberCell.CellState.RedEnabled;
                    upperLeftColor = ScoreboardNumberCell.CellState.RedEnabled;
                    centerColor = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
            }

            ScoreboardNumberCell top = new(ScoreboardNumberCell.CellOrientation.Top, topColor);
            ScoreboardNumberCell upperRight = new(ScoreboardNumberCell.CellOrientation.Right, upperRightColor);
            ScoreboardNumberCell lowerRight = new(ScoreboardNumberCell.CellOrientation.Right, lowerRightColor);
            ScoreboardNumberCell bottom = new(ScoreboardNumberCell.CellOrientation.Bottom, bottomColor);
            ScoreboardNumberCell lowerLeft = new(ScoreboardNumberCell.CellOrientation.Left, lowerLeftColor);
            ScoreboardNumberCell upperLeft = new(ScoreboardNumberCell.CellOrientation.Left, upperLeftColor);
            ScoreboardNumberCell center = new(ScoreboardNumberCell.CellOrientation.Center, centerColor);

            Children.Clear();

            Children.Add(top);
            Children.Add(upperRight);
            Children.Add(lowerRight);
            Children.Add(bottom);
            Children.Add(lowerLeft);
            Children.Add(upperLeft);
            Children.Add(center);

            AbsoluteLayout.SetLayoutBounds(top, new Rect(1, 0, 9, 3));
            AbsoluteLayout.SetLayoutBounds(upperRight, new Rect(8, 1, 3, 9));
            AbsoluteLayout.SetLayoutBounds(lowerRight, new Rect(8, 11, 3, 9));
            AbsoluteLayout.SetLayoutBounds(bottom, new Rect(1, 18, 9, 3));
            AbsoluteLayout.SetLayoutBounds(lowerLeft, new Rect(0, 11, 3, 9));
            AbsoluteLayout.SetLayoutBounds(upperLeft, new Rect(0, 1, 3, 9));
            AbsoluteLayout.SetLayoutBounds(center, new Rect(1, 9, 9, 3));
        }
    }
}
