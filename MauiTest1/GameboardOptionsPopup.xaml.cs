namespace MauiTest1;

public partial class GameboardOptionsPopup : ContentView
{
	private bool isOpen = false;
	public GameboardOptionsPopup()
	{
		IsVisible = false;
		InitializeComponent();
        MessagingCenter.Subscribe<GameboardCell, GameboardCellOptions>(this, "CellClick", (sender, arg) => { GameboardCellClicked(arg); });
    }

	private void GameboardCellClicked(GameboardCellOptions options)
	{
		int newX = options.ParentX + (options.XPosition * 16) - 28;
        int newY = options.ParentY + (options.YPosition * 16) - 36;

		int xWidthCalc = (options.XBoardSize * 16) + (options.ParentX * 2) - 14;
		if (newX + 70 > xWidthCalc) newX = xWidthCalc - 70;
        if (newX < 14) newX = 14;

		AbsoluteLayout.SetLayoutBounds(PopupGridWrapper, new Rect(newX, newY, 70, 38));
		
		if (!isOpen) IsVisible = !IsVisible;
		isOpen = true;
        
		return;
	}
}