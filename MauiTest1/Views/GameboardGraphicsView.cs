using Microsoft.Maui.Controls;
using System.ComponentModel;
using Monitoring;

namespace MauiTest1;

public class GameboardGraphicsView : GraphicsView
{
    private GameboardCanvas canvas;

	public GameboardGraphicsView()
	{
        canvas = new GameboardCanvas(BoardSetup.BoardWidth, BoardSetup.BoardHeight);

        Margin = 4;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
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

        //Recorder.Start();
        Invalidate();
        //Recorder.Stop();
    }
}