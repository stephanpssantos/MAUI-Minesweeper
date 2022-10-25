using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Reflection; // Assembly
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiTest1.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>

    static internal WindowId MainWindowId;
    static internal WindowId ScoreWindowId;
    static internal WindowId NewScoreWindowId;
    static internal WindowId CustomGameWindowId;

    public App()
	{
        InitializeComponent();

        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            // Will extend into title bar by default
            // Set to false so the app icon can be set
            nativeWindow.ExtendsContentIntoTitleBar = false;
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            // This icon is automatically converted to a .ico file and added to the
            // build folder from (I assume) the resources/appicon folder
            string iconName = "appicon.ico";
            string path = Assembly.GetExecutingAssembly().Location;
            string filePath = Path.Combine(Path.GetDirectoryName(path), iconName);
            appWindow.SetIcon(filePath);

            if (appWindow.Title == "Fasetest Mine Sweepers")
            {
                ScoreWindowId = windowId;
                ResizeHighScoreWindow(); // Refactor
            }
            else if (appWindow.Title == "New High Score")
            {
                NewScoreWindowId = windowId;
                ResizeNewHighScoreWindow(); // Refactor
            }
            else if (appWindow.Title == "Custom Game Settings")
            {
                CustomGameWindowId = windowId;
                ResizeCustomGameWindow(); // Refactor
            }
            else
            {
                MainWindowId = windowId;
                ResizeMainWindow();
            }

            nativeWindow.Activate();
        });
    }

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    static void ResizeMainWindow()
    {
        AppWindow appWindow = AppWindow.GetFromWindowId(MainWindowId);
        if (appWindow == null) return;

        int boardWidth;
        int boardHeight;
        int appWindowWidth;
        int appWindowHeight;

        switch (LocalConfig.ConfigJson.LastGameDifficulty)
        {
            case "Intermediate":
                boardWidth = 16;
                boardHeight = 16;
                break;
            case "Expert":
                boardWidth = 30;
                boardHeight = 16;
                break;
            case "Custom":
                boardWidth = LocalConfig.ConfigJson.CustomBoardWidth;
                boardHeight = LocalConfig.ConfigJson.CustomBoardHeight;
                break;
            case "Beginner":
            default:
                boardWidth = 8;
                boardHeight = 8;
                break;
        }

        appWindowWidth = (boardWidth * 16) + 44;
        appWindowHeight = (boardHeight * 16) + 146;

        appWindowWidth = appWindowWidth < 340 ? 340 : appWindowWidth;
        appWindowHeight = appWindowHeight < 332 ? 332 : appWindowHeight;

        appWindow.Resize(new SizeInt32(appWindowWidth, appWindowHeight));

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
        };

        MessagingCenter.Subscribe<GameStateViewModel, GameboardSetup>(appWindow, "Gameboard", (sender, arg) =>
        {
            ResizeGameWindow(appWindow, arg);
        });
    }

    static void ResizeHighScoreWindow()
    {
        AppWindow appWindow = AppWindow.GetFromWindowId(ScoreWindowId);
        if (appWindow == null) return;

        appWindow.Resize(new SizeInt32(370, 270));
        
        //Unnecessary
        //appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
            presenter.IsAlwaysOnTop = true;
            //Properties below do not work as expected
            //presenter.IsMaximizable = false;
            //presenter.IsMinimizable = false;
            //presenter.SetBorderAndTitleBar(true, false);
        };

        appWindow.Move(GetGameWindowOffset());
    }

    static void ResizeCustomGameWindow()
    {
        AppWindow appWindow = AppWindow.GetFromWindowId(CustomGameWindowId);
        if (appWindow == null) return;

        appWindow.Resize(new SizeInt32(350, 300));

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
            presenter.IsAlwaysOnTop = true;
        };

        appWindow.Move(GetGameWindowOffset());
    }

    static void ResizeNewHighScoreWindow()
    {
        AppWindow appWindow = AppWindow.GetFromWindowId(NewScoreWindowId);
        if (appWindow == null) return;

        appWindow.Resize(new SizeInt32(350, 280));

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
            presenter.IsAlwaysOnTop = true;
        };

        appWindow.Move(GetGameWindowOffset());
    }

    static PointInt32 GetGameWindowOffset()
    {
        PointInt32 offset = new();
        AppWindow appWindow = AppWindow.GetFromWindowId(MainWindowId);
        if (appWindow == null) return offset;
        offset = appWindow.Position;
        return offset;
    }

    static void ResizeGameWindow(AppWindow winuiAppWindow, GameboardSetup setup)
    {
        int width = (setup.BoardWidth * 16) + 44;
        int height = (setup.BoardHeight * 16) + 146;

        width = width < 340 ? 340 : width;
        height = height < 332 ? 332 : height;

        winuiAppWindow.Resize(new SizeInt32(width, height));
    }
}

