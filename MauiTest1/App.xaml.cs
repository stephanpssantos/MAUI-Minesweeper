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
		window.Deactivated += OnDeactivated;
		window.Activated += OnActivated;
		if (window != null)
		{
			window.Title = "[TODO: icon] Minesweeper";
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
