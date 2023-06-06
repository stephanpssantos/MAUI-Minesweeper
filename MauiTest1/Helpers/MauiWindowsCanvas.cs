using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;

namespace MauiTest1.Helpers
{
    public static class MauiWindowsCanvas
    {
        public static void DrawString(
            this W2DCanvas canvas, 
            Microsoft.UI.Xaml.Media.FontFamily font, 
            Windows.UI.Color color,
            int fontSize,
            string value,
            float x,
            float y,
            float width,
            float height,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            TextFlow textFlow = TextFlow.ClipBounds,
            float lineAdjustment = 0)
        {
#if !WINDOWS
            return;
#endif
#if DEBUG
            try
            {
#endif
                var textFormat = new CanvasTextFormat() { FontFamily = font.Source, FontSize = fontSize };
                textFormat.VerticalAlignment = CanvasVerticalAlignment.Top;

                Vector2 _point1 = new Vector2(x, y);

                switch (horizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        textFormat.HorizontalAlignment = CanvasHorizontalAlignment.Left;
                        break;
                    case HorizontalAlignment.Center:
                        textFormat.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        textFormat.HorizontalAlignment = CanvasHorizontalAlignment.Right;
                        break;
                    case HorizontalAlignment.Justified:
                        textFormat.HorizontalAlignment = CanvasHorizontalAlignment.Justified;
                        break;
                }

                switch (verticalAlignment)
                {
                    case VerticalAlignment.Top:
                        textFormat.VerticalAlignment = CanvasVerticalAlignment.Top;
                        break;
                    case VerticalAlignment.Center:
                        textFormat.VerticalAlignment = CanvasVerticalAlignment.Center;
                        break;
                    case VerticalAlignment.Bottom:
                        textFormat.VerticalAlignment = CanvasVerticalAlignment.Bottom;
                        break;
                }

                // Initialize a TextLayout
                var textLayout = new CanvasTextLayout(
                    canvas.Session,
                    value,
                    textFormat,
                    width,
                    height)
                {
                    Options = textFlow == TextFlow.OverflowBounds
                        ? CanvasDrawTextOptions.Default
                        : CanvasDrawTextOptions.Clip
                };
                
                var brush = new CanvasSolidColorBrush(canvas.Session, color);

                canvas.Session.DrawTextLayout(textLayout, _point1, brush);

#if DEBUG

            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
#endif
        }
    }
}
