namespace MauiTest1;

public partial class SmileyButton : ContentView
{
	public SmileyButton()
	{
		InitializeComponent();
        MessagingCenter.Subscribe<GameboardCell, int>(this, "GameButtonShockFace", (sender, args) => { DisplayFace(args); });
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
        MessagingCenter.Send<SmileyButton>(this, "NewGame");
    }

    private void DisplayFace(int faceId)
    {
        string sourceName;

        switch (faceId)
        {
            case 1:
                sourceName = "shocked_face.png";
                break;
            case 2:
                sourceName = "smiley_shades.png";
                break;
            case 0:
            default:
                sourceName = "smiley_face.png";
                break;
        }

        UnframedSmileyButton.Source = sourceName;
    }
}