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

        public void Draw(ICanvas canvas)
        {
            int x = this.xPosition * this.cellType.Size;
            int y = this.yPosition * this.cellType.Size;
            canvas.Translate(x, y);

            Rect position = new Rect(0, 0, this.CellType.Size, this.CellType.Size);
            canvas.Font = new Microsoft.Maui.Graphics.Font("8Bit", 800); // (12/6/2022) bugged. Font family cannot be set.

            if (this.cellType.TypeID == 3 && this.internalValue > 0)
            {
                canvas.FontColor = this.internalValue switch
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
                canvas.DrawString(this.internalValue.ToString(), position, HorizontalAlignment.Center, VerticalAlignment.Center);   
            }
            // This is a temporary solution.
            // It's only in place because drawing images on a canvas is currently unsupported.
            else if (this.cellType.TypeID == 1)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("?", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.cellType.TypeID == 2)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("!", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.cellType.TypeID == 4)
            {
                canvas.FontColor = Colors.Black;
                canvas.DrawString("X", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.cellType.TypeID == 5)
            {
                canvas.FontColor = Colors.Red;
                canvas.DrawString("X", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
            else if (this.cellType.TypeID == 6)
            {
                canvas.FontColor = Colors.Red;
                canvas.DrawString("!", position, HorizontalAlignment.Center, VerticalAlignment.Center);
            }

            canvas.FillColor = cellType.TopFill;
            canvas.FillPath(cellType.TopShape);

            canvas.FillColor = cellType.BottomFill;
            canvas.FillPath(cellType.BottomShape);

            canvas.Translate(-x, -y);
        }
    }

    //public class GameboardCellOptions
    //{
    //public int BoardWidth { get; set; }
    //public int BoardHeight { get; set; }
    //public int XPosition { get; set; }
    //public int YPosition { get; set; }
    //public int ParentX { get; set; }
    //public int ParentY { get; set; }
    //public int CellIndex { get; set; }
    //}
}
