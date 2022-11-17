namespace MauiTest1
{
    // Convert this into a singleton
    public class CellFactory
    {
        private Dictionary<int, CellType> cellTypes = new();

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
