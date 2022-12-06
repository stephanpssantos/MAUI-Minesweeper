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

        // This should just be a click indicator GameboardOptionsPopup(?)
        GameBoardState[cellIndex] = new CellShape(cellX, cellY, CellFactory.Instance.GetCellType(1));
    }

    private void RegenerateBoard(object sender, EventArgs e)
    {
        if (!this.IsLoaded) return;
        if (e is not PropertyChangedEventArgs args) return;
        if (args.PropertyName != "BoardSetup") return;
        
        canvas.Height = BoardSetup.BoardHeight;
        canvas.Width = BoardSetup.BoardWidth;
        this.WidthRequest = 16 * BoardSetup.BoardWidth;
        this.HeightRequest = 16 * BoardSetup.BoardHeight;
        ObservableCollection<CellShape> newShapes = new();

        for (int y = 0; y < BoardSetup.BoardHeight; y++)
        {
            for (int x = 0; x < BoardSetup.BoardWidth; x++)
            {
                newShapes.Add(new CellShape(x, y, CellFactory.Instance.GetCellType(0)));
            }
        }

        // I'm not sure, but it's possible that I should remove previous additions
        // to GameBoardState.CollectionChanged before adding new ones.
        GameBoardState = newShapes;
        canvas.Shapes = GameBoardState;
        GameBoardState.CollectionChanged += RedrawBoard; 
    }

    private void RedrawBoard(object sender, NotifyCollectionChangedEventArgs e)
    {
        Invalidate();
    }
}