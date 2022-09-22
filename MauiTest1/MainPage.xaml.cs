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



    private void InitiateClock(object sender, EventArgs e)
    {
        GameTimer.InitiateClock(BindingContext);
    }
}

