using Microsoft.Maui.Controls.Shapes;

namespace MauiTest1
{
    public class CellType
    {
        private int size;
        private int topBorderDepth;
        private int bottomBorderDepth;
        private Color topFill;
        private Color bottomFill;
        private PointCollection topShape;
        private PointCollection bottomShape;

        public CellType(int size, int topBorderDepth, int bottomBorderDepth, Color topFill, Color bottomFill)
        {
            this.size = size;
            this.topBorderDepth = topBorderDepth;
            this.bottomBorderDepth = bottomBorderDepth;
            this.topFill = topFill;
            this.bottomFill = bottomFill;
            this.Draw();
        }

        public PointCollection TopShape { get { return topShape; } }
        public PointCollection BottomShape { get { return bottomShape; } }
        public Color TopFill { get { return topFill; } }
        public Color BottomFill { get { return bottomFill; } }

        public void Draw()
        {
            PointCollection new0 = new()
            {
                new Point(0, 0),
                new Point(size, 0),
                new Point(size - topBorderDepth, topBorderDepth),
                new Point(topBorderDepth, topBorderDepth),
                new Point(topBorderDepth, size - topBorderDepth),
                new Point(0, size)
            };

            PointCollection new1 = new()
            {
                new Point(size, 0),
                new Point(size - bottomBorderDepth, bottomBorderDepth),
                new Point(size - bottomBorderDepth, size - bottomBorderDepth),
                new Point(bottomBorderDepth, size - bottomBorderDepth),
                new Point(0, size),
                new Point(size, size)
            };

            topShape = new0;
            bottomShape = new1;
        }
    }
}
