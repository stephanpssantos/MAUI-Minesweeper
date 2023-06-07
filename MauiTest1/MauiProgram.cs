using Microsoft.Extensions.Logging;

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
                fonts.AddFont("8BitWonder-Regular.ttf", "8Bit");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
