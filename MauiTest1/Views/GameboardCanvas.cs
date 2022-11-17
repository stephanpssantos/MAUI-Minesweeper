using Monitoring;

namespace MauiTest1
{
    public class GameboardCanvas : IDrawable
    {
        private CellFactory factory;

        public GameboardCanvas()
        {
            factory = new ();
        }

        public void Draw(ICanvas canvas, RectF someRect)
        {
            //Recorder.Start();
            factory.MakeCellType(1, 16, 3, 3, Color.FromArgb("#FFDDDDDD"), Color.FromArgb("#FF999999"));
            CellType basicCellType = factory.GetCellType(1);

            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    CellShape newCellShape = new CellShape(x, y, basicCellType);
                    newCellShape.Draw(canvas);
                }
            }
            //Recorder.Stop();
        }
    }
}
