namespace MauiTest1;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);
		if (window != null)
		{
            window.Deactivated += OnDeactivated;
            window.Activated += OnActivated;
            window.Title = "Minesweeper";
		}
		return window;
	}

    private void OnDeactivated(object sender, EventArgs e)
    {
        MessagingCenter.Send<Application>(this, "WindowStopped");
    }

    private void OnActivated(object sender, EventArgs e)
	{
        MessagingCenter.Send<Application>(this, "WindowResumed");
    }
}
