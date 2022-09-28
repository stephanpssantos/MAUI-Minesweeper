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

        // Markup written in XAML crashes when compiling this property for release.
        // Workaround is to set this property in code.
        ToolbarContainer.ZIndex = 1;
        PopupContainer.ZIndex = 1;
    }
}

