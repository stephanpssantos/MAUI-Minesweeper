namespace MauiTest1.Helpers
{
    /// <summary>
    /// Originally from: 
    /// https://github.com/xtuzy/Yang.Maui.Helper/blob/master/Yang.Maui.Helper/Graphics/MauiFont.cs
    /// https://github.com/dotnet/maui/issues/9252
    /// 
    /// For use when rendering graphics on a canvas.
    /// Microsoft.Maui.Graphics.Font does not properly load custom fonts.
    /// </summary>
    public class MauiCanvasFont : IDisposable
    {
        /// <summary>
        /// Get MAUI's FontManager through the View
        /// </summary>
        public View View;

#if WINDOWS
        public Microsoft.UI.Xaml.Media.FontFamily PlatformFont;
#elif IOS || MACCATALYST
        UIKit.UIFont PlatformFont;
#elif ANDROID
        Android.Graphics.Typeface PlatformFont;
#else
        object PlatformFont;
#endif

        public MauiCanvasFont(View view, Microsoft.Maui.Font font)
        {
            this.View = view;
            this.VirtualFont = font;
        }

        public Microsoft.Maui.Font VirtualFont { get; private set; }

#if WINDOWS
        public Microsoft.UI.Xaml.Media.FontFamily GetPlatformFont()
        {
            if (this.PlatformFont == null)
            {
                var fontManager = this.View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                this.PlatformFont = fontManager?.GetFontFamily(VirtualFont);
            }
            return this.PlatformFont;
        }
#elif IOS || MACCATALYST
        Dictionary<float, CoreText.CTFont> PlatformFontCache = new Dictionary<float, CoreText.CTFont>();
        float lastRequestSize = 0;
        public UIKit.UIFont GetPlatformFont_UIFont(float defaultFontSize = 0)
        {
            if (PlatformFont == null || lastRequestSize != defaultFontSize)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetFont(VirtualFont, defaultFontSize);
                lastRequestSize = defaultFontSize;
            }
            return PlatformFont;
        }

        public CoreText.CTFont GetPlatformFont_CTFont(float defaultFontSize = 0)
        {
            if (PlatformFont == null)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetFont(VirtualFont, defaultFontSize);
                lastRequestSize = defaultFontSize;
            }

            if (PlatformFontCache.ContainsKey(defaultFontSize))
            {
                return PlatformFontCache[defaultFontSize];
            }
            else
            {
                var newFont = new CoreText.CTFont(PlatformFont?.Name, defaultFontSize, CoreText.CTFontOptions.Default);
                PlatformFontCache.Add(defaultFontSize, newFont);
                return newFont;
            }
        }
#elif ANDROID
        public Android.Graphics.Typeface GetPlatformFont()
        {
            if (PlatformFont == null)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetTypeface(VirtualFont);
            }
            return PlatformFont;
        }
#endif
        public void Dispose()
        {
            this.View = null;
            this.PlatformFont = null; // Obtained from MAUI's font manager. It should dispose it itself.
#if IOS || MACCATALYST
            var keys = PlatformFontCache.Keys;
            foreach ( var key in keys )
            {
                var font = PlatformFontCache[key];
                PlatformFontCache[key] = null;
                font.Dispose();
            }
            PlatformFontCache.Clear();
#endif
        }
    }
}
