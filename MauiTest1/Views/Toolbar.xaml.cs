using System.ComponentModel; // PropertyChangedEventArgs

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

        PropertyChanged += ToggleGameDifficulty;

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
        OnToolbarButtonClicked(GameButton, null);

        if (sender == gameMenuBeginnerButton)
        {
            if (difficultySetting == "Beginner") return;
            Gameboard = GameboardSetupFactory.NewBeginnerSetup();
        }
        else if (sender == gameMenuIntermediateButton)
        {
            if (difficultySetting == "Intermediate") return;
            Gameboard = GameboardSetupFactory.NewIntermediateSetup();
        }
        else if (sender == gameMenuExpertButton)
        {
            if (difficultySetting == "Expert") return;
            Gameboard = GameboardSetupFactory.NewExpertSetup();
        }
        else if (sender == gameMenuCustomButton)
        {
            OpenCustomGameWindow();
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
        OnToolbarButtonClicked(GameButton, null); // Close game menu
        OpenBestTimesWindow();
    }

    private void OnGameMenuExitButtonClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<Toolbar>(this, "ExitGame");
    }

    private void ToggleGameDifficulty(object sender, EventArgs e)
    {
        if (e is not PropertyChangedEventArgs args) return;
        if (args.PropertyName != "Gameboard") return;
        if (Gameboard.BoardPreset == difficultySetting) return;
        
        ResetGameMenuDifficultyButtons();

        if (Gameboard.BoardPreset == "Beginner")
        {
            difficultySetting = "Beginner";
            gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Intermediate")
        {
            difficultySetting = "Intermediate";
            gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Expert")
        {
            difficultySetting = "Expert";
            gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Custom")
        {
            difficultySetting = "Custom";
            gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
    }

    private void ResetGameMenuDifficultyButtons()
    {
        gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
    }

    private void OpenCustomGameWindow()
    {
        Window secondWindow = new(new CustomGamePage())
        {
            Title = "Custom Game Settings",
        };
        Application.Current.OpenWindow(secondWindow);
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