namespace MauiTest1;

public partial class SmileyButton : ContentView
{
	public SmileyButton()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty ClockIsRunningProperty =
            BindableProperty.Create(nameof(ClockIsRunning), typeof(bool), typeof(SmileyButton), false);

    public bool ClockIsRunning
    {
        get { return (bool)GetValue(ClockIsRunningProperty); }
        set { SetValue(ClockIsRunningProperty, value); }
    }

    private void OnUnframedSmileyButtonPressed(object sender, EventArgs e)
    {
        SmileyButtonUpDiagonal.IsVisible = false;
        SmileyButtonDownDiagonal.IsVisible = true;
        UnframedSmileyButton.Padding = new Thickness(3, 3, 0, 0);
    }

    private void OnUnframedSmileyButtonReleased(object sender, EventArgs e)
    {
        SmileyButtonUpDiagonal.IsVisible = true;
        SmileyButtonDownDiagonal.IsVisible = false;
        UnframedSmileyButton.Padding = new Thickness(2, 2, 1, 1);

        ClockIsRunning = !ClockIsRunning;
    }
}