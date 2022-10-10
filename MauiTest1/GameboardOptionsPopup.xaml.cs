namespace MauiTest1;

public partial class GameboardOptionsPopup : ContentView
{
	private bool isOpen = false;
	private GameboardCell lastClicked;

	public GameboardOptionsPopup()
	{
		IsVisible = false;
		InitializeComponent();
        MessagingCenter.Subscribe<GameboardCell, GameboardCellOptions>(this, "CellClick", (sender, arg) => {
			if (sender is GameboardCell clicked)
			{
				if (clicked == lastClicked)
				{
                    MessagingCenter.Send<GameboardCell, int>(lastClicked, "SmileyFace", 0);
                    ClosePopup(this, null);
					return;
				}
                GameboardCellClicked(arg);
                lastClicked = clicked;
			}
        });

        MessagingCenter.Subscribe<OptionsPopupCell>(this, "ClosePopup", (sender) => { ClosePopup(null, null); });
    }

	private void ClosePopupButtonClicked(object sender, EventArgs e)
	{
        MessagingCenter.Send<GameboardCell, int>(lastClicked, "SmileyFace", 0);
        ClosePopup(this, null);
	}

	private void GameboardCellClicked(GameboardCellOptions options)
	{
		if (isOpen && lastClicked is not null)
		{
            lastClicked.ResetCellStatus();
		}

		ClearButton.xPosition = options.XPosition;
		ClearButton.yPosition = options.YPosition;
        MarkButton.xPosition = options.XPosition;
        MarkButton.yPosition = options.YPosition;
        FlagButton.xPosition = options.XPosition;
        FlagButton.yPosition = options.YPosition;
        CancelButton.xPosition = options.XPosition;
        CancelButton.yPosition = options.YPosition;

        // 27 = 16 block size + (16 / 2) to center + 3 padding of one side
        int newX = options.ParentX + (options.XPosition * 16) - 27;
        // 38 = 16 block size * 2 rows + 6 padding
        int newY = options.ParentY + (options.YPosition * 16) - 38;

		// 14 = 10 padding + 4 border (?)
		int xWidthCalc = (options.XBoardSize * 16) + (options.ParentX * 2) - 14;
		if (newX + 70 > xWidthCalc) newX = xWidthCalc - 70;
        if (newX < 14) newX = 14;

		AbsoluteLayout.SetLayoutBounds(PopupGridWrapper, new Rect(newX, newY, 70, 38));
		
		if (!isOpen) IsVisible = !IsVisible;
		isOpen = true;
        
		return;
	}

	private void ClosePopup(object sender, EventArgs e)
	{
		if (sender is not null && lastClicked is not null)
		{
            lastClicked.ResetCellStatus();
        }

        isOpen = false;
		IsVisible = false;
		lastClicked = null;
	}
}