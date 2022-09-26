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
                    //context.GameMode = GameStateViewModel.GameModeSetting.Expert;
                    //context.Gameboard = GameboardSetupFactory.NewExpertSetup();
                }
                else
                {
                    GameTimer.StopClock();
                    //context.GameMode = GameStateViewModel.GameModeSetting.Beginner;
                    //context.Gameboard = GameboardSetupFactory.NewIntermediateSetup();
                }
                break;
            default:
                break;
        }
    }
}

