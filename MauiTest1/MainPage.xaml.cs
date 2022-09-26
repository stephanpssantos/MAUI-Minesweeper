using System.ComponentModel;

namespace MauiTest1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
        GameTimer Timer = new();
        GameStateViewModel GameState = new();
        BindingContext = GameState;    
    }
}

