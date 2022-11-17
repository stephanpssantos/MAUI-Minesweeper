namespace MauiTest1
{
    public class OldCellFactory
    {
        private Dictionary<string, CellType> cellTypes = new();

        public CellType GetCellType(int size, int topBorderDepth, int bottomBorderDepth)
        {
            string cellhash = GetCellHash(size, topBorderDepth, bottomBorderDepth);

            if (cellTypes.ContainsKey(cellhash)) 
            {
                return cellTypes[cellhash];
            }
            else
            {
                //CellType newCellType = new(size, topBorderDepth, bottomBorderDepth);
                //cellTypes.Add(cellhash, newCellType);
                //return newCellType;
            }
        }

        private string GetCellHash(int size, int topBorderDepth, int bottomBorderDepth)
        {
            return size.ToString() + "-" + topBorderDepth.ToString() + "-" + bottomBorderDepth.ToString();
        }
    }
}
