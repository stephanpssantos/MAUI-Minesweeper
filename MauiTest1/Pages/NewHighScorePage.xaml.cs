namespace MauiTest1;

public partial class NewHighScorePage : ContentPage
{
	private string difficulty = "Custom";

	public NewHighScorePage()
	{
		InitializeComponent();
		OKButton.Pressed += OnOKButtonPressed;
	}

	public string Difficulty 
	{ 
		get { return difficulty; } 
		set 
		{ 
			difficulty = value;
            VictoryText.Text = $"You have the fastest time for {difficulty.ToLower()} level. Please enter your name.";
        } 
	}

	private void OnOKButtonPressed(object sender, EventArgs e)
	{
		SetRecordName();
	}

	private void SetRecordName()
	{
		if (string.IsNullOrEmpty(RecordName.Text))
		{
			RecordName.Text = "Anonymous";
		}

		switch (difficulty)
		{
			case "Beginner":
				LocalConfig.ConfigJson.BegginerName = RecordName.Text;
				break;
			case "Intermediate":
                LocalConfig.ConfigJson.IntermediateName = RecordName.Text;
                break;
			case "Expert":
                LocalConfig.ConfigJson.ExpertName = RecordName.Text;
                break;
		}

		LocalConfig.OverwriteConfig();
		
		Window parentWindow = GetParentWindow();

        Window secondWindow = new(new HighScoresPage())
        {
            Title = "Fasetest Mine Sweepers"
        };
        Application.Current.OpenWindow(secondWindow);

        Application.Current.CloseWindow(parentWindow);
    }
}