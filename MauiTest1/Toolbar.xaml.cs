namespace MauiTest1;

public partial class Toolbar : ContentView
{
    Button activeButton;
    bool marks = false;
    bool color = false;
    string difficultySetting = "Beginner";

    public Toolbar()
	{
		InitializeComponent();
        GameMenu.IsVisible = false;
        HelpMenu.IsVisible = false;

        MessagingCenter.Subscribe<GameboardCell, GameboardCellOptions>(this, "CellClick", (sender, arg) =>
        {
            if (GameMenu.IsVisible == true) GameMenu.IsVisible = false;
            if (HelpMenu.IsVisible == true) HelpMenu.IsVisible = false;
            GameButton.Style = (Style)Resources["neutralToolbarButtonStyle"];
            HelpButton.Style = (Style)Resources["neutralToolbarButtonStyle"];
            activeButton = null;
        });
    }

    public static readonly BindableProperty GameboardProperty =
            BindableProperty.Create(nameof(Gameboard), 
                typeof(GameboardSetup), 
                typeof(Toolbar), 
                GameboardSetupFactory.NewBeginnerSetup());

    public GameboardSetup Gameboard
    {
        get { return (GameboardSetup)GetValue(GameboardProperty); }
        set { SetValue(GameboardProperty, value); }
    }

    private void OnToolbarButtonClicked(object sender, EventArgs e)
    {
        Button triggeredControl = sender as Button;

        // Refactor this
        if (triggeredControl == GameButton)
        {
            if (activeButton == GameButton)
            {
                GameMenu.IsVisible = false;
            }
            else
            {
                GameMenu.IsVisible = true;
                HelpMenu.IsVisible = false;
            }
        }
        else if (triggeredControl == HelpButton)
        {
            if (activeButton == HelpButton)
            {
                HelpMenu.IsVisible = false;
            }
            else
            {
                HelpMenu.IsVisible = true;
                GameMenu.IsVisible = false;
            }
        }

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

        MessagingCenter.Send<Toolbar>(this, "ClosePopup");
    }

    private void OnGameMenuNewButtonClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<Toolbar>(this, "NewGame");
        OnToolbarButtonClicked(GameButton, null);
    }

    // Refactor these
    private void OnGameMenuDifficultyButtonClicked(object sender, EventArgs e)
    {
        ResetGameMenuDifficultyButtons();
        OnToolbarButtonClicked(GameButton, null);

        if (sender == gameMenuBeginnerButton)
        {
            gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];

            if (difficultySetting == "Beginner") return;

            difficultySetting = "Beginner";
            Gameboard = GameboardSetupFactory.NewBeginnerSetup();
        }
        else if (sender == gameMenuIntermediateButton)
        {
            gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];

            if (difficultySetting == "Intermediate") return;

            difficultySetting = "Intermediate";
            Gameboard = GameboardSetupFactory.NewIntermediateSetup();
        }
        else if (sender == gameMenuExpertButton)
        {
            gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];

            if (difficultySetting == "Expert") return;

            difficultySetting = "Expert";
            Gameboard = GameboardSetupFactory.NewExpertSetup();
        }
        else if (sender == gameMenuCustomButton)
        {
            gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];

            if (difficultySetting == "Custom") return;

            difficultySetting = "Custom";
            Gameboard = GameboardSetupFactory.NewBeginnerSetup();
        }
        else
        {
            // Do something?
        }

        LocalConfig.ConfigJson.LastGameDifficulty = difficultySetting;
        LocalConfig.OverwriteConfig();
    }

    private void OnGameMenuMarksButtonClicked(object sender, EventArgs e)
    {
        if (marks)
        {
            marks = false;
            gameMenuMarksCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        }
        else
        {
            marks = true;
            gameMenuMarksCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
    }

    private void OnGameMenuColorButtonClicked(object sender, EventArgs e)
    {
        if (color)
        {
            color = false;
            gameMenuColorCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        }
        else
        {
            color = true;
            gameMenuColorCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
    }

    private void OnGameMenuBestTimesButtonClicked(object sender, EventArgs e)
    {
        OpenBestTimesWindow();
        OnToolbarButtonClicked(GameButton, null); // Close game menu
    }

    private void OnGameMenuExitButtonClicked(object sender, EventArgs e)
    {
    }

    private void ResetGameMenuDifficultyButtons()
    {
        gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
    }

    private void OpenBestTimesWindow()
    {
        Window secondWindow = new(new HighScoresPage())
        {
            Title = "Fasetest Mine Sweepers"
        };
        Application.Current.OpenWindow(secondWindow);
    }
}