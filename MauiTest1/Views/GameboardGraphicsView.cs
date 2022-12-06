using Microsoft.Maui.Controls;
using System.ComponentModel;
using Monitoring;
using System.Collections.ObjectModel;

namespace MauiTest1;

public class GameboardGraphicsView : GraphicsView
{
    private GameboardCanvas canvas;
    private ObservableCollection<CellShape> shapes; // Maybe this can be moved to GameboardState

    public GameboardGraphicsView()
	{
        canvas = new GameboardCanvas(BoardSetup.BoardWidth, BoardSetup.BoardHeight);
        Drawable = canvas;
        PropertyChanged += RegenerateBoard;
    }

    public static readonly BindableProperty BoardSetupProperty =
        BindableProperty.Create(nameof(BoardSetup), typeof(GameboardSetup), typeof(GameboardGraphicsView), GameboardSetupFactory.NewBeginnerSetup());

    public GameboardSetup BoardSetup
    {
        get { return (GameboardSetup)GetValue(BoardSetupProperty); }
        set { SetValue(BoardSetupProperty, value); }
    }

    private void RegenerateBoard(object sender, EventArgs e)
    {
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

        shapes = newShapes;
        canvas.Shapes = shapes;

        //Recorder.Start();
        Invalidate();
        //Recorder.Stop();
    }
}