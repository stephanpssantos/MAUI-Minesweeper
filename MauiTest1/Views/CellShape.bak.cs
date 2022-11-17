using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using MauiTest1.Helpers;

namespace MauiTest1
{
    public class OldCellShape
    {
        private int xPosition = 0;
        private int yPosition = 0;
        private CellType cellType;

        public OldCellShape(int x, int y, CellType cellType)
        {
            this.xPosition = x;
            this.yPosition = y;
            this.cellType = cellType;
        }

        public bool IsClickable
        {
            get { return true; }
        }

        public void Draw(Layout canvas)
        {
            PointCollection new0 = new()
            {
                new Point(xPosition, yPosition),
                new Point(xPosition + cellType.Size, yPosition),
                new Point(xPosition + cellType.Size - cellType.TopBorderDepth, cellType.TopBorderDepth),
                new Point(cellType.TopBorderDepth, cellType.TopBorderDepth),
                new Point(cellType.TopBorderDepth, yPosition + cellType.Size - cellType.TopBorderDepth),
                new Point(xPosition, yPosition + cellType.Size)
            };

            PointCollection new1 = new()
            {
                new Point(xPosition + cellType.Size, yPosition),
                new Point(xPosition + cellType.Size - cellType.BottomBorderDepth, cellType.BottomBorderDepth),
                new Point(xPosition + cellType.Size - cellType.BottomBorderDepth, yPosition + cellType.Size - cellType.BottomBorderDepth),
                new Point(cellType.BottomBorderDepth, yPosition + cellType.Size - cellType.BottomBorderDepth),
                new Point(xPosition, yPosition + cellType.Size),
                new Point(xPosition + cellType.Size, yPosition + cellType.Size)
            };

            Polygon shape1 = new() { Fill = Colors.Gray };
            Polygon shape2 = new() { Fill = Colors.DarkGray };

            shape1.Points = new0;
            shape2.Points = new1;

            canvas.Children.Add(shape1);
            canvas.Children.Add(shape2);
        }
    }
}
