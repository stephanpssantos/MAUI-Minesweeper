#if WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif

using MauiTest1.Helpers;

namespace MauiTest1
{
    public class CellShape
    {
        private int xPosition = 0;
        private int yPosition = 0;
        private int internalValue = 0;
        private CellType cellType;

        public CellShape(int x, int y, int internalValue, CellType cellType)
        {
            this.xPosition = x;
            this.yPosition = y;
            this.internalValue = internalValue;
            this.cellType = cellType;
        }

        public int InternalValue
        {
            get { return internalValue; }
        }

        public CellType CellType
        {
            get { return cellType; }
            set { cellType = value; }
        }

        public CellShape Clone()
        {
            CellShape clone = new CellShape(xPosition, yPosition, internalValue, cellType);
            return clone;
        }

        public void Draw(ICanvas canvas, GameboardGraphicsView parentView)
        {
            int x = this.xPosition * this.CellType.Size;
            int y = this.yPosition * this.CellType.Size;
            canvas.Translate(x, y);

            Rect position = new Rect(0, 0, this.CellType.Size, this.CellType.Size);

            // canvas.Font is bugged for windows.
            canvas.Font = new Microsoft.Maui.Graphics.Font("8Bit", 800);
#if WINDOWS
            // Any number is fine for font size. It won't be used. We're using this method only 
            // because it's the only available method of getting a custom Maui.Font.
            Microsoft.Maui.Font customFont = Microsoft.Maui.Font.OfSize("8Bit", 12);
            MauiCanvasFont canvasFont = new MauiCanvasFont(parentView, customFont);     
#endif

            if (this.CellType.TypeID == 3 && this.internalValue > 0)
            {
                Color fontColor = this.internalValue switch
                {
                    1 => Colors.Blue,
                    2 => Colors.Green,
                    3 => Colors.Red,
                    4 => Colors.DarkBlue,
                    5 => Colors.Maroon,
                    6 => Colors.DarkViolet,
                    7 => Colors.Brown,
                    8 => Colors.Gold,
                    _ => Colors.Black,
                };
#if WINDOWS
                fontColor.ToRgba(out byte r, out byte g, out byte b, out byte a);
                Windows.UI.Color winColor = Windows.UI.Color.FromArgb(a, r, g, b);
                Microsoft.UI.Xaml.Media.FontFamily winFont = canvasFont.GetPlatformFont();
                W2DCanvas winCanvas = canvas as W2DCanvas;
                winCanvas.DrawString(winFont, 
                    winColor,
                    12,
                    this.internalValue.ToString(), 
                    (float)position.X, 
                    (float)position.Y, 
                    (float)position.Width, 
                    (float)position.Height, 
                    HorizontalAlignment.Center, 
                    VerticalAlignment.Center);
#else
                canvas.FontColor = fontColor;
                canvas.DrawString(this.internalValue.ToString(), position, HorizontalAlignment.Center, VerticalAlignment.Center);   
#endif
            }
            else if (this.CellType.TypeID == 1)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("?", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.CellType.TypeID == 2)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("!", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.CellType.TypeID == 4)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("X", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.CellType.TypeID == 5)
            {
                canvas.FontColor = Colors.Red;
                canvas.DrawString("X", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.CellType.TypeID == 6)
            {
                canvas.FontColor = Colors.Red;
                canvas.DrawString("!", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }

            canvas.FillColor = CellType.TopFill;
            canvas.FillPath(CellType.TopShape);

            canvas.FillColor = CellType.BottomFill;
            canvas.FillPath(CellType.BottomShape);

            canvas.Translate(-x, -y);
        }
    }
}
