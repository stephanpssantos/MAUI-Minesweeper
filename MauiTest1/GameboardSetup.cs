namespace MauiTest1
{
    public class GameboardSetup
    {
        public GameboardSetup(int boardWidth = 8, int boardHeight = 8, int boardMines = 10)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoardMines = boardMines;
        }

        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BoardMines { get; set; }
    }

    public static class GameboardSetupFactory
    {
        public static GameboardSetup NewBeginnerSetup()
        {
            return new GameboardSetup(8, 8, 10);
        }

        public static GameboardSetup NewIntermediateSetup()
        {
            return new GameboardSetup(16, 16, 40);
        }

        public static GameboardSetup NewExpertSetup()
        {
            return new GameboardSetup(30, 16, 99);
        }

        public static GameboardSetup NewCustomSetup(int width, int height, int mines)
        {
            return new GameboardSetup(width, height, mines);
        }
    }
}
