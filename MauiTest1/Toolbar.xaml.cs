namespace MauiTest1;

public partial class Toolbar : ContentView
{
    Button activeButton;
    bool isBeginner = true;
    bool isIntermediate = false;
    bool isExpert = false;
    bool isCustom = false;
    bool marks = false;
    bool color = false;

    public Toolbar()
	{
		InitializeComponent();
        GameMenu.IsVisible = false;
        HelpMenu.IsVisible = false;
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
            isBeginner = true;
            gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (sender == gameMenuIntermediateButton)
        {
            isIntermediate = true;
            gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (sender == gameMenuExpertButton)
        {
            isExpert = true;
            gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
        }
        else if (sender == gameMenuCustomButton)
        {
            isCustom = true;
            gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckboxChecked"];
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
        isBeginner = false;
        isIntermediate = false;
        isExpert = false;
        isCustom = false;

        gameMenuBeginnerCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuIntermediateCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuExpertCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
        gameMenuCustomCheckbox.Style = (Style)Resources["toolbarMenuCheckbox"];
    }
}