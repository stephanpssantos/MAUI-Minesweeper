using System.ComponentModel; // PropertyChangedEventArgs

namespace MauiTest1
{
    public class Scoreboard : AbsoluteLayout
    {
        public Scoreboard()
        {
            RenderScoreboard();
            PropertyChanged += ChangeScore;
        }

        public static readonly BindableProperty NumberProperty =
            BindableProperty.Create(nameof(Number), typeof(string), typeof(Scoreboard), "000");

        public string Number
        {
            get
            {
                string temp = (string)GetValue(NumberProperty);
                if (temp.Length < 3)
                {
                    temp = temp.PadLeft(3, '0');
                    SetValue(NumberProperty, temp);
                }
                return temp;
            }
            set { SetValue(NumberProperty, value); }
        }

        private void RenderScoreboard()
        {
            string tempNumber = Number;

            DiagonalBlockShape border = new(2);
            Children.Add(border);
            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 0, 42, 26));

            // - '0' is to convert the char number to an int. Int32.Parse doesn't work.
            ScoreboardNumber scoreboardNumber1 = new() { Number = tempNumber[0] - '0' };
            ScoreboardNumber scoreboardNumber2 = new() { Number = tempNumber[1] - '0' };
            ScoreboardNumber scoreboardNumber3 = new() { Number = tempNumber[2] - '0' };

            Children.Add(scoreboardNumber1);
            Children.Add(scoreboardNumber2);
            Children.Add(scoreboardNumber3);

            AbsoluteLayout.SetLayoutBounds(scoreboardNumber1, new Rect(2, 1, 13, 24));
            AbsoluteLayout.SetLayoutBounds(scoreboardNumber2, new Rect(15, 1, 13, 24));
            AbsoluteLayout.SetLayoutBounds(scoreboardNumber3, new Rect(28, 1, 13, 24));
        }

        private void ChangeScore(object sender, EventArgs e)
        {
            if (e is not PropertyChangedEventArgs args) return;

            if (args.PropertyName != "Number") return;

            if (Children.Count < 4)
            {
                return;
            }

            ScoreboardNumber scoreboardNumber1 = Children[1] as ScoreboardNumber;
            ScoreboardNumber scoreboardNumber2 = Children[2] as ScoreboardNumber;
            ScoreboardNumber scoreboardNumber3 = Children[3] as ScoreboardNumber;

            scoreboardNumber1.Number = Number[0] - '0';
            scoreboardNumber2.Number = Number[1] - '0';
            scoreboardNumber3.Number = Number[2] - '0';
        }
    }
}
