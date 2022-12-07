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
        GameButton.Clicked += OnToolbarButtonClicked;
        HelpButton.Clicked += OnToolbarButtonClicked;
        GameMenuNewButton.Clicked += OnGameMenuNewButtonClicked;
        GameMenuBeginnerButton.Clicked += OnGameMenuDifficultyButtonClicked;
        GameMenuIntermediateButton.Clicked += OnGameMenuDifficultyButtonClicked;
        GameMenuExpertButton.Clicked += OnGameMenuDifficultyButtonClicked;
        GameMenuCustomButton.Clicked += OnGameMenuDifficultyButtonClicked;
        GameMenuMarksButton.Clicked += OnGameMenuMarksButtonClicked;
        GameMenuColorButton.Clicked += OnGameMenuColorButtonClicked;
        GameMenuBestTimesButton.Clicked += OnGameMenuBestTimesButtonClicked;
        GameMenuExitButton.Clicked += OnGameMenuExitButtonClicked;

        MessagingCenter.Subscribe<GameboardCell, GameboardCellOptions>(this, "CellClick", (sender, arg) =>
        {
            if (GameMenu.IsVisible == true) GameMenu.IsVisible = false;
            if (HelpMenu.IsVisible == true) HelpMenu.IsVisible = false;
            GameButton.Style = (Style)Resources["local_neutralToolbarButtonStyle"];
            HelpButton.Style = (Style)Resources["local_neutralToolbarButtonStyle"];
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
            triggeredControl.Style = (Style)Resources["local_neutralToolbarButtonStyle"];
        }
        else
        {
            if (activeButton != null)
            {
                activeButton.Style = (Style)Resources["local_neutralToolbarButtonStyle"];
            }
            activeButton = triggeredControl;
            activeButton.Style = (Style)Resources["local_openToolbarButtonStyle"];
        }

        MessagingCenter.Send<Toolbar>(this, "ClosePopup");
    }

    private void OnGameMenuNewButtonClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send<Application, GameboardSetup>(Application.Current, "NewGame", null);
        OnToolbarButtonClicked(GameButton, null);
    }

    // Refactor these
    private void OnGameMenuDifficultyButtonClicked(object sender, EventArgs e)
    {
        OnToolbarButtonClicked(GameButton, null);

        if (sender == GameMenuBeginnerButton)
        {
            if (difficultySetting == "Beginner") return;
            Gameboard = GameboardSetupFactory.NewBeginnerSetup();
        }
        else if (sender == GameMenuIntermediateButton)
        {
            if (difficultySetting == "Intermediate") return;
            Gameboard = GameboardSetupFactory.NewIntermediateSetup();
        }
        else if (sender == GameMenuExpertButton)
        {
            if (difficultySetting == "Expert") return;
            Gameboard = GameboardSetupFactory.NewExpertSetup();
        }
        else if (sender == GameMenuCustomButton)
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
            GameMenuMarksCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
        }
        else
        {
            marks = true;
            GameMenuMarksCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
        }
    }

    private void OnGameMenuColorButtonClicked(object sender, EventArgs e)
    {
        if (color)
        {
            color = false;
            GameMenuColorCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
        }
        else
        {
            color = true;
            GameMenuColorCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
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
            GameMenuBeginnerCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Intermediate")
        {
            difficultySetting = "Intermediate";
            GameMenuIntermediateCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Expert")
        {
            difficultySetting = "Expert";
            GameMenuExpertCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
        }
        else if (Gameboard.BoardPreset == "Custom")
        {
            difficultySetting = "Custom";
            GameMenuCustomCheckbox.Style = (Style)Resources["local_toolbarMenuCheckboxChecked"];
        }
    }

    private void ResetGameMenuDifficultyButtons()
    {
        GameMenuBeginnerCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
        GameMenuIntermediateCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
        GameMenuExpertCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
        GameMenuCustomCheckbox.Style = (Style)Resources["local_toolbarMenuCheckbox"];
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