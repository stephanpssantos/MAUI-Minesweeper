namespace MauiTest1;

public partial class Toolbar : ContentView
{
    Button activeButton;
    bool marks = false;
    bool color = false;

    public Toolbar()
	{
		InitializeComponent();
        GameMenu.IsVisible = false;
        HelpMenu.IsVisible = false;
    }

    public static readonly BindableProperty GameModeProperty =
            BindableProperty.Create(nameof(GameMode), 
                typeof(GameStateViewModel.GameModeSetting), 
                typeof(Toolbar), 
                GameStateViewModel.GameModeSetting.Beginner);

    public GameStateViewModel.GameModeSetting GameMode
    {
        get { return (GameStateViewModel.GameModeSetting)GetValue(GameModeProperty); }
        set { SetValue(GameModeProperty, value); }
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
    }

    private void OnGameMenuNewButtonClicked(object sender, EventArgs e)
    {
    }

    // Refactor these
    private void OnGameMenuDifficultyButtonClicked(object sender, EventArgs e)
    {
        ResetGameMenuDifficultyButtons();

        if (sender == gameMenuBeginnerButton)
        {
            gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
            if (GameMode == GameStateViewModel.GameModeSetting.Beginner) return;
            GameMode = GameStateViewModel.GameModeSetting.Beginner;
        }
        else if (sender == gameMenuIntermediateButton)
        {
            gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
            if (GameMode == GameStateViewModel.GameModeSetting.Intermediate) return;
            GameMode = GameStateViewModel.GameModeSetting.Intermediate;
        }
        else if (sender == gameMenuExpertButton)
        {
            gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
            if (GameMode == GameStateViewModel.GameModeSetting.Expert) return;
            GameMode = GameStateViewModel.GameModeSetting.Expert;
        }
        else if (sender == gameMenuCustomButton)
        {
            gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
            if (GameMode == GameStateViewModel.GameModeSetting.Custom) return;
            GameMode = GameStateViewModel.GameModeSetting.Custom;
        }
        else
        {
            // Do something?
        }
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
}