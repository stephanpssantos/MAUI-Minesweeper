namespace MauiTest1
{
    public class GameboardSetup
    {
        public GameboardSetup(int boardWidth = 8, int boardHeight = 8, int boardMines = 10, string boardPreset = "Custom")
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoardMines = boardMines;
            BoardPreset = boardPreset;
            RandomizeBoardMines();
        }

        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BoardMines { get; set; }
        public int[,] BoardPositions { get; set; }
        public string BoardPreset { get; set; }

        private void RandomizeBoardMines()
        {
            Random rand = new();
            BoardPositions = new int[BoardWidth,BoardHeight];
            int mineCount = 0;

            while (mineCount < BoardMines)
            {
                int x;
                int y;
                do
                {
                    x = rand.Next(0, BoardWidth);
                    y = rand.Next(0, BoardHeight);
                }
                while (BoardPositions[x, y] != 0);

                BoardPositions[x, y] = -1;
                mineCount++;
            }

            CalculateBoardPositions();
        }

        private void CalculateBoardPositions()
        {
            for (int i = 0; i < BoardWidth; i++)
            {
                for (int j = 0; j < BoardHeight; j++)
                {
                    if (BoardPositions[i, j] == -1) continue;

                    int count = 0;
                    // left
                    if (i - 1 >= 0)
                    {
                        if (BoardPositions[i - 1, j] == -1) count++;
                    }
                    // top left
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        if (BoardPositions[i - 1, j - 1] == -1) count++;
                    }
                    // top
                    if (j - 1 >= 0)
                    {
                        if (BoardPositions[i, j - 1] == -1) count++;
                    }
                    // top right
                    if (i + 1 < BoardWidth && j - 1 >= 0)
                    {
                        if (BoardPositions[i + 1, j - 1] == -1) count++;
                    }
                    // right
                    if (i + 1 < BoardWidth)
                    {
                        if (BoardPositions[i + 1, j] == -1) count++;
                    }
                    // bottom right
                    if (i + 1 < BoardWidth && j + 1 < BoardHeight)
                    {
                        if (BoardPositions[i + 1, j + 1] == -1) count++;
                    }
                    // bottom
                    if (j + 1 < BoardHeight)
                    {
                        if (BoardPositions[i, j + 1] == -1) count++;
                    }
                    // bottom left
                    if (i - 1 >= 0 && j + 1 < BoardHeight)
                    {
                        if (BoardPositions[i - 1, j + 1] == -1) count++;
                    }

                    BoardPositions[i, j] = count;
                }
            }
        }
    }

    public static class GameboardSetupFactory
    {
        public static GameboardSetup NewBeginnerSetup()
        {
            return new GameboardSetup(8, 8, 10, "Beginner");
        }

        public static GameboardSetup NewIntermediateSetup()
        {
            return new GameboardSetup(16, 16, 40, "Intermediate");
        }

        public static GameboardSetup NewExpertSetup()
        {
            return new GameboardSetup(30, 16, 99, "Expert");
        }

        public static GameboardSetup NewCustomSetup(int width, int height, int mines)
        {
            return new GameboardSetup(width, height, mines, "Custom");
        }
    }
}
