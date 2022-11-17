namespace MauiTest1
{
    public sealed class CellFactory
    {
        private static readonly Lazy<CellFactory> lazy = new Lazy<CellFactory>(() => new CellFactory());
        private static Dictionary<int, CellType> cellTypes = new();

        private CellFactory() { }

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
                CellType newCellType = new(size, topBorderDepth, bottomBorderDepth, topFill, bottomFill);
                cellTypes.Add(id, newCellType);
            }
        }
    }
}
