namespace MauiTest1;

public partial class MainPage : ContentPage
{
	Button activeButton;

	public MainPage()
	{
		InitializeComponent();
		//Resources["gameButtonStyle"] = Resources["neutralToolbarButtonStyle"];
		//Resources["helpButtonStyle"] = Resources["neutralToolbarButtonStyle"];
    }

	private void OnToolbarButtonClicked(object sender, EventArgs e)
	{
        Button triggeredControl = sender as Button;

		if (triggeredControl == activeButton)
		{
			activeButton = null;
            triggeredControl.Style = (Style)Resources["neutralToolbarButtonStyle"];
        }
		else
		{
			if (activeButton != null)
			{
                activeButton.Style = (Style)Resources["neutralToolbarButtonStyle"];
            }
            activeButton = triggeredControl;
            activeButton.Style = (Style)Resources["openToolbarButtonStyle"];
        }
    }
}

