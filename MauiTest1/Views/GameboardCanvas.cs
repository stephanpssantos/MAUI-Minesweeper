using Monitoring;

namespace MauiTest1
{
    public class GameboardCanvas : IDrawable
    {
        private CellFactory factory;

        public GameboardCanvas()
        {
            CellFactory.Instance.MakeCellType(1, 16, 3, 3, Color.FromArgb("#FFDDDDDD"), Color.FromArgb("#FF999999"));
            CellFactory.Instance.MakeCellType(2, 16, 3, 3, Color.FromArgb("#FFAADDAA"), Color.FromArgb("#FF009900"));
        }

        public void Draw(ICanvas canvas, RectF rectF)
        {
            //Recorder.Start();
            CellType basicCellType = CellFactory.Instance.GetCellType(1);
            CellType alternateCellType = CellFactory.Instance.GetCellType(2);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    CellShape newCellShape = new CellShape(x, y, basicCellType);
                    newCellShape.Draw(canvas);
                }
            }

            CellShape shape1 = new CellShape(0, 0, alternateCellType);
            shape1.Draw(canvas);
            CellShape shape2 = new CellShape(5, 5, alternateCellType);
            shape2.Draw(canvas);
            CellShape shape3 = new CellShape(12, 6, alternateCellType);
            shape3.Draw(canvas);
            //Recorder.Stop();
        }
    }
}
