namespace MauiTest1;

public partial class GameboardOptionsPopup : ContentView
{
	private bool isOpen = false;
	private GameboardCellOptions lastClicked;

	public GameboardOptionsPopup()
	{
		IsVisible = false;
		InitializeComponent();

        ClosePopupButton.Pressed += ClosePopupButtonPressed;

        MessagingCenter.Subscribe<OptionsPopupCell>(this, "ClosePopup", (sender) => { ClosePopup(null, null); });
        MessagingCenter.Subscribe<Toolbar>(this, "ClosePopup", (sender) => { ClosePopup(this, null); });
        MessagingCenter.Subscribe<GameboardGraphicsView, GameboardCellOptions>(this, "CellClick", (sender, arg) => { GameboardCellClicked(arg); });
    }

	private void ClosePopupButtonPressed(object sender, EventArgs e)
	{
        MessagingCenter.Send<Application, int>(Application.Current, "SmileyFace", 0);
        ClosePopup(this, null);
	}

    private void GameboardCellClicked(GameboardCellOptions options)
    {
        if (lastClicked is not null && options.XPosition == lastClicked.XPosition && options.YPosition == lastClicked.YPosition)
        {
            MessagingCenter.Send<Application, int>(Application.Current, "SmileyFace", 0);
            ClosePopup(this, null);
            return;
        }
        OpenPopup(options);
        lastClicked = options;
    }

	private void OpenPopup(GameboardCellOptions options)
	{
        if (isOpen && lastClicked is not null)
        {
            MessagingCenter.Send<Application, int>(Application.Current, "ResetCell", lastClicked.CellIndex);
        }

        // This is so when one of these buttons is clicked, it gets a
        // reference to the active gameboard cell.
        ClearButton.XPosition = options.XPosition;
		ClearButton.YPosition = options.YPosition;
        MarkButton.XPosition = options.XPosition;
        MarkButton.YPosition = options.YPosition;
        FlagButton.XPosition = options.XPosition;
        FlagButton.YPosition = options.YPosition;
        CancelButton.XPosition = options.XPosition;
        CancelButton.YPosition = options.YPosition;

        // 26 = 16 block size + (16 / 2) to center + 2 padding of one side
        int newX = options.ParentX + (options.XPosition * 16) - 26;
        // 37 = 16 block size * 2 rows + 5 padding
        int newY = options.ParentY + (options.YPosition * 16) - 37;

		// 13 = 10 padding + 3 border (?)
		int xWidthCalc = (options.BoardWidth * 16) + (options.ParentX * 2) - 13;
		if (newX + 70 > xWidthCalc) newX = xWidthCalc - 70;
        if (newX < 14) newX = 13;

		AbsoluteLayout.SetLayoutBounds(PopupGridWrapper, new Rect(newX, newY, 70, 38));
		
		if (!isOpen) IsVisible = !IsVisible;
		isOpen = true;
        
		return;
	}

	private void ClosePopup(object sender, EventArgs e)
	{
        if (sender is not null && lastClicked is not null)
        {
            MessagingCenter.Send<Application, int>(Application.Current, "ResetCell", lastClicked.CellIndex);
        }

        isOpen = false;
		IsVisible = false;
		lastClicked = null;
	}
}