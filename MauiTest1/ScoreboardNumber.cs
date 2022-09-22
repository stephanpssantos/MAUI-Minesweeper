using System.ComponentModel; // PropertyChangedEventArgs

namespace MauiTest1
{
    public class ScoreboardNumber : AbsoluteLayout
    {
        public ScoreboardNumber()
        {
            BackgroundColor = Colors.Black;
            Padding = 1;
            HeightRequest = 23;
            WidthRequest = 13;

            RenderNumber();
            PropertyChanged += ChangeNumber;
        }

        public static readonly BindableProperty NumberProperty =
            BindableProperty.Create(nameof(Number), typeof(int), typeof(ScoreboardNumber), 0);

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        private void ChangeNumber(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;

            if (args.PropertyName != "Number") return;

            if (Children.Count < 1) return;

            ScoreboardNumberCell top = Children[0] as ScoreboardNumberCell;
            ScoreboardNumberCell upperRight = Children[1] as ScoreboardNumberCell;
            ScoreboardNumberCell lowerRight = Children[2] as ScoreboardNumberCell;
            ScoreboardNumberCell bottom = Children[3] as ScoreboardNumberCell;
            ScoreboardNumberCell lowerLeft = Children[4] as ScoreboardNumberCell;
            ScoreboardNumberCell upperLeft = Children[5] as ScoreboardNumberCell;
            ScoreboardNumberCell center = Children[6] as ScoreboardNumberCell;

            ScoreboardPositions positions = DetermineScoreboardPositions(Number);

            top.ThisState = positions.top;
            upperRight.ThisState = positions.upperRight;
            lowerRight.ThisState = positions.lowerRight;
            bottom.ThisState = positions.bottom;
            lowerLeft.ThisState = positions.lowerLeft;
            upperLeft.ThisState = positions.upperLeft;
            center.ThisState = positions.center;
        }

        private void RenderNumber()
        {
            if (Number < 0 || Number > 9)
            {
                return;
            }

            ScoreboardPositions positions = DetermineScoreboardPositions(Number);

            ScoreboardNumberCell top = new(ScoreboardNumberCell.CellOrientation.Top, positions.top);
            ScoreboardNumberCell upperRight = new(ScoreboardNumberCell.CellOrientation.Right, positions.upperRight);
            ScoreboardNumberCell lowerRight = new(ScoreboardNumberCell.CellOrientation.Right, positions.lowerRight);
            ScoreboardNumberCell bottom = new(ScoreboardNumberCell.CellOrientation.Bottom, positions.bottom);
            ScoreboardNumberCell lowerLeft = new(ScoreboardNumberCell.CellOrientation.Left, positions.lowerLeft);
            ScoreboardNumberCell upperLeft = new(ScoreboardNumberCell.CellOrientation.Left, positions.upperLeft);
            ScoreboardNumberCell center = new(ScoreboardNumberCell.CellOrientation.Center, positions.center);

            // Children.Clear();

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

        public ScoreboardPositions DetermineScoreboardPositions (int number)
        {
            ScoreboardPositions positions = new();
            switch (Number)
            {
                case 0:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 1:
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 2:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 3:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 4:
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 5:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 6:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 7:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 8:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
                case 9:
                default:
                    positions.top = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.lowerRight = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.bottom = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.upperLeft = ScoreboardNumberCell.CellState.RedEnabled;
                    positions.center = ScoreboardNumberCell.CellState.RedEnabled;
                    break;
            }

            return positions;
        }
}

    public class ScoreboardPositions {
        public ScoreboardNumberCell.CellState top { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState upperRight { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState lowerRight { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState bottom { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState lowerLeft { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState upperLeft { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
        public ScoreboardNumberCell.CellState center { get; set; } = ScoreboardNumberCell.CellState.RedDisabled;
    }
}
