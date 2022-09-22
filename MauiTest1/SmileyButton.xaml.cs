namespace MauiTest1;

public partial class SmileyButton : ContentView
{
	public SmileyButton()
	{
		InitializeComponent();
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
    }
}