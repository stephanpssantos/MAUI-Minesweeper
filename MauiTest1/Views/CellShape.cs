using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using MauiTest1.Helpers;
using System.IO;

namespace MauiTest1
{
    public class CellShape
    {
        private int xPosition = 0;
        private int yPosition = 0;
        private CellType cellType;

        public CellShape(int x, int y, CellType cellType)
        {
            this.xPosition = x;
            this.yPosition = y;
            this.cellType = cellType;
        }

        public bool IsClickable
        {
            get { return true; }
        }

        public void Draw(ICanvas canvas)
        {
            int x = this.xPosition * this.cellType.Size;
            int y = this.yPosition * this.cellType.Size;
            canvas.Translate(x, y);

            canvas.FillColor = cellType.TopFill;
            canvas.FillPath(cellType.TopShape);

            canvas.FillColor = cellType.BottomFill;
            canvas.FillPath(cellType.BottomShape);

            canvas.Translate(-x, -y);
        }
    }
}
