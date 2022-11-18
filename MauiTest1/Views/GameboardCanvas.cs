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

        public CellShape[,] Shapes { get; set; }

        public void Draw(ICanvas canvas, RectF rectF)
        {
            CellType basicCellType = CellFactory.Instance.GetCellType(0);
            CellType fancyCellType = CellFactory.Instance.GetCellType(1);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    CellShape newCellShape = new CellShape(x, y, basicCellType);
                    newCellShape.Draw(canvas);
                }
            }
        }
    }
}
