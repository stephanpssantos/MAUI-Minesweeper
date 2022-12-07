using Microsoft.Maui.Controls;
using System.ComponentModel;
using Monitoring;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MauiTest1;

public class GameboardGraphicsView : GraphicsView
{
    private GameboardCanvas canvas;

    public GameboardGraphicsView()
	{
        canvas = new GameboardCanvas(BoardSetup.BoardWidth, BoardSetup.BoardHeight);
        Drawable = canvas;
        Loaded += GameboardGraphicsViewLoaded;
        PropertyChanged += RegenerateBoard;
        StartInteraction += GameboardGraphicsViewStartInteraction;
    }

    public static readonly BindableProperty GameBoardStateProperty =
        BindableProperty.Create(nameof(GameBoardState), typeof(ObservableCollection<CellShape>), typeof(GameboardGraphicsView));

    public static readonly BindableProperty BoardSetupProperty =
        BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(GameboardGraphicsView), GameboardSetupFactory.NewBeginnerSetup());

    public ObservableCollection<CellShape> GameBoardState
    {
        get { return (ObservableCollection<CellShape>)GetValue(GameBoardStateProperty); }
        set { SetValue(GameBoardStateProperty, value); }
    }

    public GameboardSetup BoardSetup
    {
        get { return (GameboardSetup)GetValue(BoardSetupProperty); }
        set { SetValue(BoardSetupProperty, value); }
    }

    private void GameboardGraphicsViewLoaded(object sender, EventArgs e)
    {
        this.RegenerateBoard(this, new PropertyChangedEventArgs("BoardSetup"));
    }

    private void GameboardGraphicsViewStartInteraction(object sender, TouchEventArgs e)
    {
        PointF clickLocation = e.Touches.FirstOrDefault();
        int cellX = (int)clickLocation.X / 16;
        int cellY = (int)clickLocation.Y / 16;
        int cellIndex = (cellY * BoardSetup.BoardWidth) + cellX;

        // Prevent clicks on non-open cells.
        if (GameBoardState[cellIndex].CellType.TypeID == 3) return;

        // This has to be a clone so that GameBoardState.CollectionChanged triggers.
        CellShape newCell = GameBoardState[cellIndex].Clone();
        newCell.CellType = CellFactory.Instance.GetCellType(7);
        GameBoardState[cellIndex] = newCell;

        GameboardCellOptions options = new()
        {
            BoardWidth = BoardSetup.BoardWidth,
            BoardHeight = BoardSetup.BoardHeight,
            XPosition = cellX,
            YPosition = cellY,
            ParentX = (int)this.X,
            ParentY = (int)this.Y,
            CellIndex = cellIndex
        };

        MessagingCenter.Send<Application, int>(Application.Current, "SmileyFace", 1);
        MessagingCenter.Send<GameboardGraphicsView, GameboardCellOptions>(this, "CellClick", options);
    }

    private void RegenerateBoard(object sender, EventArgs e)
    {
        if (!this.IsLoaded) return;
        if (e is not PropertyChangedEventArgs args) return;
        if (args.PropertyName != "BoardSetup") return;

        // Clear old board (if any)
        if (GameBoardState is not null)
        {
            GameBoardState.CollectionChanged -= RedrawBoard;
            Invalidate();
        }

        canvas.Height = BoardSetup.BoardHeight;
        canvas.Width = BoardSetup.BoardWidth;
        this.WidthRequest = 16 * BoardSetup.BoardWidth;
        this.HeightRequest = 16 * BoardSetup.BoardHeight;
        ObservableCollection<CellShape> newShapes = new();

        for (int y = 0; y < BoardSetup.BoardHeight; y++)
        {
            for (int x = 0; x < BoardSetup.BoardWidth; x++)
            {
                int internalValue = BoardSetup.BoardPositions[x, y];
                newShapes.Add(new CellShape(x, y, internalValue, CellFactory.Instance.GetCellType(0)));
            }
        }

        GameBoardState = newShapes;
        canvas.Shapes = GameBoardState;
        GameBoardState.CollectionChanged += RedrawBoard; 
    }

    private void RedrawBoard(object sender, NotifyCollectionChangedEventArgs e)
    {
        Invalidate();
    }
}