using MauiTest1.Helpers;

namespace MauiTest1
{
    public sealed class CellFactory
    {
        private static readonly Lazy<CellFactory> lazy = new Lazy<CellFactory>(() => new CellFactory());
        private static Dictionary<int, CellType> cellTypes = new();

        private CellFactory() 
        {
            // Drawing images on canvases is currently broken.
            // Eventually, these CellTypes should have distinct images.
            // For now, I'm using text to distinguish the different states.
            // TODO: Use stored resources for the colors
            this.MakeCellType(0, 16, 2, 2, Color.FromArgb("#FFFFFFFF"), Color.FromArgb("#FF808080")); // Default cell style
            this.MakeCellType(1, 16, 2, 2, Color.FromArgb("#FFFFFFFF"), Color.FromArgb("#FF808080")); // Question mark
            this.MakeCellType(2, 16, 2, 2, Color.FromArgb("#FFFFFFFF"), Color.FromArgb("#FF808080")); // Flagged
            this.MakeCellType(3, 16, 1, 0, Color.FromArgb("#FF808080"), Color.FromArgb("#FF000000")); // Open 
            this.MakeCellType(4, 16, 1, 0, Color.FromArgb("#FF808080"), Color.FromArgb("#FF000000")); // Exploded
            this.MakeCellType(5, 16, 1, 0, Color.FromArgb("#FF808080"), Color.FromArgb("#FF000000")); // Explosion
            this.MakeCellType(7, 16, 1, 0, Color.FromArgb("#FF808080"), Color.FromArgb("#FF000000")); // Pressed
        }

        public static CellFactory Instance { get { return lazy.Value; } }

        public CellType GetCellType(int id)
        {
            if (cellTypes.ContainsKey(id)) 
            {
                return cellTypes[id];
            }
            else
            {
                // Log an exception
                return null;
            }
        }

        public void MakeCellType(int id, int size, int topBorderDepth, int bottomBorderDepth, Color topFill, Color bottomFill)
        {
            if (cellTypes.ContainsKey(id)) 
            {
                // Log an exception
                return;
            }
            else
            {
                CellType newCellType = new(id, size, topBorderDepth, bottomBorderDepth, topFill, bottomFill);
                cellTypes.Add(id, newCellType);
            }
        }
    }
}
