using Microsoft.UI;
using Microsoft.UI.Windowing;
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
	public App()
	{
        InitializeComponent();

        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(364, 466));

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
            };

            MessagingCenter.Subscribe<GameStateViewModel, GameboardSetup>(appWindow, "Gameboard", (sender, arg) =>
            {
                ResizeWindow(appWindow, arg);
            });
        });
    }

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    static void ResizeWindow(AppWindow winuiAppWindow, GameboardSetup setup)
    {
        int width = (setup.BoardWidth * 16) + 108;
        int height = (setup.BoardHeight * 16) + 210;
        winuiAppWindow.Resize(new SizeInt32(width, height));
    }
}

