namespace MauiTest1;

public partial class SmileyButton : ContentView
{
    private bool gameOver = false;

	public SmileyButton()
	{
		InitializeComponent();

        UnframedSmileyButton.Pressed += OnUnframedSmileyButtonPressed;
        UnframedSmileyButton.Released += OnUnframedSmileyButtonReleased;

        MessagingCenter.Subscribe<Application, int>(this, "SmileyFace", (sender, args) => { DisplayFace(args); });
        MessagingCenter.Subscribe<GameStateViewModel, int>(this, "SmileyFace", (sender, args) => { DisplayFace(args); });
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

        if (gameOver == false)
        {
            ClockIsRunning = !ClockIsRunning;
        }
        
        MessagingCenter.Send<Application>(Application.Current, "NewGame");
        DisplayFace(0);
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
                gameOver = true;
                break;
            case 3:
                sourceName = "exploded_face.png";
                gameOver = true;
                break;
            case 0:
            default:
                sourceName = "smiley_face.png";
                gameOver=false;
                break;
        }

        UnframedSmileyButton.Source = sourceName;
    }
}