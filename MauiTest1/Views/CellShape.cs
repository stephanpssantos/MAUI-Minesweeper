using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using MauiTest1.Helpers;

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

        public void Draw(Layout canvas)
        {
            Polygon shape1 = new Polygon() { Fill = cellType.TopFill, Points = cellType.TopShape };
            Polygon shape2 = new Polygon() { Fill = cellType.BottomFill, Points = cellType.BottomShape };
            canvas.Children.Add(shape1);
            canvas.Children.Add(shape2);
        }
    }
}
