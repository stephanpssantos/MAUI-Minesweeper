using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml.Controls;
using Monitoring;
using System.ComponentModel;

namespace MauiTest1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
        GameTimer Timer = new();
        GameStateViewModel GameState = new();
        BindingContext = GameState;

        Recorder.Start();

        CellFactory factory = new CellFactory();
        factory.MakeCellType(1, 16, 3, 3, Colors.Red, Colors.Blue);
        CellType basicCellType = factory.GetCellType(1);

        for (int i = 0; i < 1000; i++)
        {
            CellShape newCell = new(0, 0, basicCellType);
            newCell.Draw(TestLayout2);
        }

        GraphicsView ass = new();
        PointCollection bleh = basicCellType.BottomShape;
        

        //for (int i = 0; i < 100; i++)
        //{
        //    TestLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(16) });
        //}
        //for (int i = 0; i < 100; i++)
        //{
        //    TestLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(16) });
        //}

        Recorder.Stop();

        // Markup written in XAML crashes when compiling this property for release.
        // Workaround is to set this property in code.
        ToolbarContainer.ZIndex = 1;
        PopupContainer.ZIndex = 1;

        MessagingCenter.Subscribe<Toolbar>(this, "ExitGame", (sender) => 
        {
            Window parentWindow = GetParentWindow();
            Application.Current.CloseWindow(parentWindow);
        });
    }

    public class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF someRect)
        {
            
        }
    }
}

