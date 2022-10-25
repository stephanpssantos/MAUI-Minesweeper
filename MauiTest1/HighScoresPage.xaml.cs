namespace MauiTest1;

public partial class HighScoresPage : ContentPage
{
	public HighScoresPage()
	{
		InitializeComponent();
		ReadConfigValues();
		OKButton.Pressed += OnOKButtonPressed;
		ResetButton.Pressed += OnResetButtonPressed;
	}

	private void OnOKButtonPressed(object sender, EventArgs e)
	{
        Application.Current.CloseWindow(GetParentWindow());
    }

	private void OnResetButtonPressed(object sender, EventArgs e)
	{
		ResetConfigValues();
		ReadConfigValues();
    }

	private void ReadConfigValues()
	{
		BeginnerNameLabel.Text = LocalConfig.ConfigJson.BegginerName;
		BeginnerTimeLabel.Text = LocalConfig.ConfigJson.BeginnerTime + " seconds";
		IntermediateNameLabel.Text = LocalConfig.ConfigJson.IntermediateName;
		IntermediateTimeLabel.Text = LocalConfig.ConfigJson.IntermediateTime + " seconds";
        ExpertNameLabel.Text = LocalConfig.ConfigJson.ExpertName;
		ExpertTimeLabel.Text = LocalConfig.ConfigJson.ExpertTime + " seconds";
    }

	private void ResetConfigValues()
	{
		LocalConfig.ConfigJson.BegginerName = "Anonymous";
        LocalConfig.ConfigJson.IntermediateName = "Anonymous";
        LocalConfig.ConfigJson.ExpertName = "Anonymous";

		LocalConfig.ConfigJson.BeginnerTime = 999;
		LocalConfig.ConfigJson.IntermediateTime = 999;
		LocalConfig.ConfigJson.ExpertTime = 999;

		LocalConfig.OverwriteConfig();
    }
}