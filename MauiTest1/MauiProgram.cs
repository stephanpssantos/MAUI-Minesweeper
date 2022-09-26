#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

using Microsoft.Maui.LifecycleEvents;

namespace MauiTest1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

//#if WINDOWS
//		builder.ConfigureLifecycleEvents(events =>
//		{
//			events.AddWindows(windows =>
//				windows.OnWindowCreated(window =>
//					{
//						IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
//						WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
//						AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);    
//						if(winuiAppWindow.Presenter is OverlappedPresenter p)
//						{ 
//						   p.IsResizable=false;
//						}    

//						MessagingCenter.Subscribe<GameStateViewModel, GameboardSetup>(window, "Gameboard", (sender, arg) =>
//							{
//								ResizeWindow(winuiAppWindow, arg);
//							});
//					}));
//		});
//#endif

        return builder.Build();
	}

#if WINDOWS
	static void ResizeWindow(AppWindow winuiAppWindow, GameboardSetup setup)
	{
		int width = (setup.BoardWidth * 16) + 108;
		int height = (setup.BoardHeight * 16) + 210;
		winuiAppWindow.Resize(new SizeInt32(width, height));
	}
#endif
}
