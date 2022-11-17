namespace MauiTest1
{
    public class CellType
    {
        private int size;
        private int topBorderDepth;
        private int bottomBorderDepth;
        private Color topFill;
        private Color bottomFill;
        private PathF topShape;
        private PathF bottomShape;

        public CellType(int size, int topBorderDepth, int bottomBorderDepth, Color topFill, Color bottomFill)
        {
            this.size = size;
            this.topBorderDepth = topBorderDepth;
            this.bottomBorderDepth = bottomBorderDepth;
            this.topFill = topFill;
            this.bottomFill = bottomFill;
            this.MakePaths();
        }

        public int Size { get { return size; } }
        public PathF TopShape { get { return topShape; } }
        public PathF BottomShape { get { return bottomShape; } }
        public Color TopFill { get { return topFill; } }
        public Color BottomFill { get { return bottomFill; } }

        public void MakePaths()
        {
            PathF topPath = new PathF();
            topPath.MoveTo(0, 0);
            topPath.LineTo(size, 0);
            topPath.LineTo(size - topBorderDepth, topBorderDepth);
            topPath.LineTo(topBorderDepth, topBorderDepth);
            topPath.LineTo(topBorderDepth, size - topBorderDepth);
            topPath.LineTo(0, size);

            PathF bottomPath = new PathF();
            bottomPath.MoveTo(size, 0);
            bottomPath.LineTo(size - bottomBorderDepth, bottomBorderDepth);
            bottomPath.LineTo(size - bottomBorderDepth, size - bottomBorderDepth);
            bottomPath.LineTo(bottomBorderDepth, size - bottomBorderDepth);
            bottomPath.LineTo(0, size);
            bottomPath.LineTo(size, size);

            topShape = topPath;
            bottomShape = bottomPath;
        }
    }
}
