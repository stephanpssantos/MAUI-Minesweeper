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

        MessagingCenter.Subscribe<GameStateViewModel>(this, "LockBoard", (sender) => 
        {
            GameboardCanvas.IsEnabled = false;
        });

        MessagingCenter.Subscribe<GameStateViewModel>(this, "UnlockBoard", (sender) =>
        {
            GameboardCanvas.IsEnabled = true;
        });

        MessagingCenter.Subscribe<Toolbar>(this, "ExitGame", (sender) => 
        {
            Window parentWindow = GetParentWindow();
            Application.Current.CloseWindow(parentWindow);
        });
    }
}

