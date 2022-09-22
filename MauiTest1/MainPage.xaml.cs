namespace MauiTest1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
        GameStateViewModel GameState = new();
        BindingContext = GameState;
    }

    private void TestIncrementValue(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (button.BindingContext is not GameStateViewModel s) return;
        int tempScoreboardNumber = Int32.Parse(s.Number);
        tempScoreboardNumber++;
        s.Number = tempScoreboardNumber.ToString();
    }

    private void OnSmileyButtonPressed(object sender, EventArgs e)
    {
        SmileyButtonUpDiagonal.IsVisible = false;
        SmileyButtonDownDiagonal.IsVisible = true;
        SmileyButton.Padding = new Thickness(3, 3, 0, 0);
        InitiateClock(this, null);
    }

    private void OnSmileyButtonReleased(object sender, EventArgs e)
    {
        SmileyButtonUpDiagonal.IsVisible = true;
        SmileyButtonDownDiagonal.IsVisible = false;
        SmileyButton.Padding = new Thickness(2, 2, 1, 1);
    }

    private void InitiateClock(object sender, EventArgs e)
    {
        GameTimer.InitiateClock(BindingContext);
    }
}

