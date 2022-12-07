namespace MauiTest1;

public partial class CustomGamePage : ContentPage
{
	public CustomGamePage()
	{
		InitializeComponent();
		OKButton.Pressed += OnOKButtonPressed;
	}

	private void OnOKButtonPressed(object sender, EventArgs e)
	{
		int width;
		int height;
		int mines;

		if (int.TryParse(WidthEntry.Text, out width))
		{
			if (width < 3 || width > 999) width = 8;
		}
		else
		{ 
			width = 8; 
		}
        if (int.TryParse(HeightEntry.Text, out height))
        {
            if (height < 3 || height > 999) height = 8;
        }
        else
        {
            height = 8;
        }
        if (int.TryParse(MineCountEntry.Text, out mines))
        {
            if (mines < 3 || mines > 999) mines = 10;
        }
        else
        {
            mines = 10;
        }

		GameboardSetup newSetup = GameboardSetupFactory.NewCustomSetup(width, height, mines);
        MessagingCenter.Send<Application, GameboardSetup>(Application.Current, "NewGame", newSetup);

        LocalConfig.ConfigJson.CustomBoardWidth = width;
        LocalConfig.ConfigJson.CustomBoardHeight = height;
        LocalConfig.ConfigJson.CustomBoardMines = mines;
        LocalConfig.ConfigJson.LastGameDifficulty = "Custom";
        LocalConfig.OverwriteConfig();

        // Closes the active window i.e. this
        Window parentWindow = GetParentWindow();
        Application.Current.CloseWindow(parentWindow);
    }
}