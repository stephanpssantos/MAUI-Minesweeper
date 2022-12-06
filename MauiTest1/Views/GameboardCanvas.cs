using System.Collections.ObjectModel; // ObservableCollection

namespace MauiTest1
{
    public class GameboardCanvas : IDrawable
    {
        public GameboardCanvas(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public int Height { get; set; }

        public int Width { get; set; }

        public ObservableCollection<CellShape> Shapes { get; set; }

        public void Draw(ICanvas canvas, RectF rectF)
        {
            int boardPosition;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    boardPosition = (y * Width) + x;
                    CellShape shape = Shapes[boardPosition];
                    shape.Draw(canvas);
                }
            }
        }
    }
}
