using System.ComponentModel;

namespace MauiTest1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
        GameStateViewModel GameState = new();
        BindingContext = GameState;
        GameState.PropertyChanged += GameStateEventHandler;      
    }

    private void GameStateEventHandler(object sender, EventArgs e)
    {
        if (e is not PropertyChangedEventArgs args) return;
        if (BindingContext is not GameStateViewModel context) return;

        switch (args.PropertyName)
        {
            case "ClockIsRunning":
                if (context.ClockIsRunning)
                {
                    GameTimer.InitiateClock(BindingContext);
                }
                else
                {
                    GameTimer.StopClock();
                }
                break;
            default:
                break;
        }
    }

    //private void TestIncrementValue(object sender, EventArgs e)
    //{
    //    Button button = sender as Button;
    //    if (button.BindingContext is not GameStateViewModel s) return;
    //    int tempScoreboardNumber = Int32.Parse(s.Number);
    //    tempScoreboardNumber++;
    //    s.Number = tempScoreboardNumber.ToString();
    //}
}

