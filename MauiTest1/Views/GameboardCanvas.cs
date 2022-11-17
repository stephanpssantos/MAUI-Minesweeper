namespace MauiTest1
{
    public class GameboardCanvas : IDrawable
    {
        private CellFactory factory;

        public GameboardCanvas()
        {
        }

        public void Draw(ICanvas canvas, RectF rectF)
        {
            CellType basicCellType = CellFactory.Instance.GetCellType(0);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    CellShape newCellShape = new CellShape(x, y, basicCellType);
                    newCellShape.Draw(canvas);
                }
            }
        }
    }
}
