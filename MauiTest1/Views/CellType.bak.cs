namespace MauiTest1
{
    public class OldCellType
    {
        private int size = 16;
        private int topBorderDepth = 5;
        private int bottomBorderDepth = 5;

        public OldCellType(int size, int topBorderDepth, int bottomBorderDepth)
        {
            this.size = size;
            this.topBorderDepth = topBorderDepth;
            this.bottomBorderDepth = bottomBorderDepth;
        }

        public int Size { get { return size; } }
        public int TopBorderDepth { get { return topBorderDepth; } }
        public int BottomBorderDepth { get { return bottomBorderDepth; } }
    }
}
