namespace MauiTest1;

public partial class MainPage : ContentPage
{
    static System.Timers.Timer timer;

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
        if (timer == null)
        {
            timer = new System.Timers.Timer(interval: 1000);
            timer.Elapsed += UpdateClock;
        }

        timer.Start();
    }

    private void UpdateClock(object sender, EventArgs e)
    {
        GameStateViewModel gameState = BindingContext as GameStateViewModel;
        int timeElapsed = Int32.Parse(gameState.TimeElapsed);
        timeElapsed++;
        string newTimeElapsed = timeElapsed.ToString().PadLeft(3, '0');
        
        gameState.TimeElapsed = newTimeElapsed;
    }
}

