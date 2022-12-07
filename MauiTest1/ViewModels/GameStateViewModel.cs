using System.Collections.ObjectModel;
using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices;

namespace MauiTest1
{
    public class GameStateViewModel : INotifyPropertyChanged
    {
        private string mineCount = "000";
        private string timeElapsed = "000";
        private bool clockIsRunning = false;
        private bool gameOver = false;
        private GameboardSetup gameboard;
        private ObservableCollection<int> gameboardState;
        private ObservableCollection<CellShape> gameboardState_new;

        public event PropertyChangedEventHandler PropertyChanged;

        // this attribute sets the propertyName parameter
        // using the context in which this method is called
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // if an event handler has been set, then invoke
            // the delegate and pass the name of the property
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameStateViewModel()
        {
            MessagingCenter.Subscribe<Application>(this, "NewGame", (sender) => { NewGame(); });
            MessagingCenter.Subscribe<GameTimer>(this, "ClockTick", (sender) => { IncrementTimeElapsed(); });
            MessagingCenter.Subscribe<Application, int>(this, "ResetCell", (sender, args) => { ResetCell(args); });
            MessagingCenter.Subscribe<OptionsPopupCell, SelectedPopupCellOptions>(this, "OptionCellClicked", (sender, args) => { PopupCellOptionClicked(args); });
            MessagingCenter.Subscribe<CustomGamePage, GameboardSetup>(this, "NewCustomGame", (sender, args) => { NewCustomGame(args); });
            MessagingCenter.Subscribe<Application>(this, "WindowStopped", (sender) => { ClockIsRunning = false; });
            MessagingCenter.Subscribe<Application>(this, "WindowResumed", (sender) => 
            {
                if (timeElapsed == "000") return;
                if (gameOver == true) return;
                ClockIsRunning = true; 
            });
            MessagingCenter.Subscribe<GameboardGraphicsView, GameboardCellOptions>(this, "CellClick", (sender, arg) =>
            {
                if (ClockIsRunning == false)
                {
                    ClockIsRunning = true;
                }
            });

            string difficulty = LocalConfig.ConfigJson.LastGameDifficulty;
            Gameboard = difficulty switch
            {
                "Intermediate" => GameboardSetupFactory.NewIntermediateSetup(),
                "Expert" => GameboardSetupFactory.NewExpertSetup(),
                "Custom" => GameboardSetupFactory.NewCustomSetup(
                                        LocalConfig.ConfigJson.CustomBoardWidth,
                                        LocalConfig.ConfigJson.CustomBoardHeight,
                                        LocalConfig.ConfigJson.CustomBoardMines
                                    ),
                _ => GameboardSetupFactory.NewBeginnerSetup(),
            };
        }

        public string MineCount
        {
            get => mineCount;
            set 
            {
                int negativeCheck = Int32.Parse(value);
                if (negativeCheck < 0)
                {
                    // possibly verify that it's less than 99
                    negativeCheck *= -1;
                    value = '-' + negativeCheck.ToString().PadLeft(2, '0');
                }
                else if (value.Length < 3)
                {
                    value = value.PadLeft(3, '0');
                }
                mineCount = value; 
                NotifyPropertyChanged(); 
            }
        }

        public string TimeElapsed
        {
            get => timeElapsed;
            set 
            {
                if (Int32.Parse(value) < 0) return;
                if (value.Length < 3)
                {
                    value = value.PadLeft(3, '0');
                }
                timeElapsed = value; 
                NotifyPropertyChanged(); 
            }
        }

        public bool ClockIsRunning
        {
            get => clockIsRunning;
            set 
            {
                clockIsRunning = value; 
                NotifyPropertyChanged();
                MessagingCenter.Send<GameStateViewModel, bool>(this, "ClockIsRunning", value);
            }
        }

        public GameboardSetup Gameboard
        {
            get => gameboard;
            set
            {
                gameboard = value;
                MineCount = gameboard.BoardMines.ToString();
                TimeElapsed = "000";
                ClockIsRunning = false;
                NotifyPropertyChanged();
                MessagingCenter.Send<GameStateViewModel, GameboardSetup>(this, "Gameboard", value);
            }
        }

        public ObservableCollection<int> GameboardState
        {
            get => gameboardState;
            set { gameboardState = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<CellShape> GameboardState_new
        {
            get => gameboardState_new;
            set { gameboardState_new = value; NotifyPropertyChanged(); }
        }

        private void NewGame()
        {
            MessagingCenter.Send<GameStateViewModel>(this, "UnlockBoard");
            gameOver = false;
            GameboardSetup newGameboardSetup = new(Gameboard.BoardWidth, Gameboard.BoardHeight, Gameboard.BoardMines, Gameboard.BoardPreset);
            Gameboard = newGameboardSetup;
        }

        private void NewCustomGame(GameboardSetup setup)
        {
            MessagingCenter.Send<GameStateViewModel>(this, "UnlockBoard");
            gameOver = false;
            Gameboard = setup;
        }

        // Used when you are setting multiple cells at once (e.g. in a loop.)
        // Set all cell values, then call this to avoid having to redraw multiple times.
        private void RedrawCells()
        {
            if (GameboardState_new is null) return;
            CellShape newCell = GameboardState_new[0].Clone();
            GameboardState_new[0] = newCell;
        }

        // Will immediately redraw.
        private void SetCellState(int cellIndex, int newState)
        {
            if (GameboardState_new is null || cellIndex >= GameboardState_new.Count) return;
            if (newState < 0 || newState > 7) return;

            CellShape newCell = GameboardState_new[cellIndex].Clone();
            newCell.CellType = CellFactory.Instance.GetCellType(newState);
            GameboardState_new[cellIndex] = newCell;
        }

        private void ResetCell(int cellIndex)
        {
            SetCellState(cellIndex, 0);
        }

        private void IncrementTimeElapsed(int value = 1)
        {
            int timeElapsed = Int32.Parse(TimeElapsed);
            if (timeElapsed == 999)
            {
                ClockIsRunning = false;
                return;
            }
            timeElapsed += value;
            TimeElapsed = timeElapsed.ToString();
        }

        private void PopupCellOptionClicked(SelectedPopupCellOptions options)
        {
            // selectedOptions has string ActionName; int XPosition; int YPosition;
            // Action names: Clear, Mark, Flag, Cancel
            int cellIndex = (options.YPosition * Gameboard.BoardWidth) + options.XPosition;
            int cellValue = Gameboard.BoardPositions[options.XPosition, options.YPosition];

            if (options.ActionName == "Clear")
            {
                if (cellValue > 0) 
                {
                    SetCellState(cellIndex, 3);
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                    CheckVictory();
                }
                else if (cellValue < 0)
                {
                    // Game over
                    gameOver = true;
                    ClockIsRunning = false;
                    ExplodeAll(options.XPosition, options.YPosition);
                    MessagingCenter.Send<GameStateViewModel>(this, "LockBoard");
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 3);
                }
                else if (cellValue == 0)
                {
                    OpenSurroundings(options.XPosition, options.YPosition);
                    this.RedrawCells();
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                }
            }
            else if (options.ActionName == "Mark")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                SetCellState(cellIndex, 1);
            }
            else if (options.ActionName == "Flag")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                int mineCountInt = Int32.Parse(MineCount);
                mineCountInt--;
                MineCount = mineCountInt.ToString();
                SetCellState(cellIndex, 2);
                if (mineCountInt == 0) CheckVictory();
            }
            else if (options.ActionName == "Cancel")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                if (GameboardState_new[cellIndex].CellType.TypeID == 2)
                {
                    int mineCountInt = Int32.Parse(MineCount);
                    mineCountInt++;
                    MineCount = mineCountInt.ToString();
                }
                SetCellState(cellIndex, 0);
            }
        }

        private void CheckVictory()
        {
            bool unflaggedMines = false;
            bool unopenedNumbers = false;

            for (int y = 0; y < Gameboard.BoardHeight; y++)
            {
                for (int x = 0; x < Gameboard.BoardWidth; x++)
                {
                    int currentCellIndex = (y * Gameboard.BoardWidth) + x;
                    int currentCellValue = Gameboard.BoardPositions[x, y];

                    // if currentCell has mine and is not flagged
                    if (currentCellValue == -1 && GameboardState_new[currentCellIndex].CellType.TypeID != 2)
                    {
                        unflaggedMines = true;
                    }
                    // if currentCell is number and is not open
                    if (currentCellValue > 0 && GameboardState_new[currentCellIndex].CellType.TypeID != 3)
                    {
                        unopenedNumbers = true;
                    }
                }
            }

            if (!unflaggedMines || !unopenedNumbers)
            {
                gameOver = true;
                ClockIsRunning = false;
                MineCount = "000";
                OpenAll();
                MessagingCenter.Send<GameStateViewModel>(this, "LockBoard");
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 2);
                CheckHighScores();
            }
        }

        private void CheckHighScores()
        {
            int timeElapsed = Int32.Parse(TimeElapsed);
            int record = Gameboard.BoardPreset switch
            {
                "Beginner" => LocalConfig.ConfigJson.BeginnerTime,
                "Intermediate" => LocalConfig.ConfigJson.IntermediateTime,
                "Expert" => LocalConfig.ConfigJson.ExpertTime,
                _ => 0
            };

            if (record == 0) return;
            if (timeElapsed > record) return;

            switch (Gameboard.BoardPreset)
            {
                case "Beginner":
                    LocalConfig.ConfigJson.BeginnerTime = timeElapsed;
                    LocalConfig.ConfigJson.BegginerName = "Anonymous";
                    break;
                case "Intermediate":
                    LocalConfig.ConfigJson.IntermediateTime = timeElapsed;
                    LocalConfig.ConfigJson.IntermediateName = "Anonymous";
                    break;
                case "Expert":
                    LocalConfig.ConfigJson.ExpertTime = timeElapsed;
                    LocalConfig.ConfigJson.ExpertName = "Anonymous";
                    break;
            }

            LocalConfig.OverwriteConfig();

            Window secondWindow = new(new NewHighScorePage() 
            { 
                Difficulty = Gameboard.BoardPreset 
            })
            {
                Title = "New High Score",
            };
            Application.Current.OpenWindow(secondWindow);
        }

        private void OpenAll()
        {
            for (int y = 0; y < Gameboard.BoardHeight; y++)
            {
                for (int x = 0; x < Gameboard.BoardWidth; x++)
                {
                    int currentCellIndex = (y * Gameboard.BoardWidth) + x;
                    int currentCellValue = Gameboard.BoardPositions[x, y];

                    if (currentCellValue == -1 && GameboardState_new[currentCellIndex].CellType.TypeID != 2)
                    {
                        GameboardState_new[currentCellIndex].CellType = CellFactory.Instance.GetCellType(2);
                    }
                    if (currentCellValue > 0 && GameboardState_new[currentCellIndex].CellType.TypeID != 3)
                    {
                        GameboardState_new[currentCellIndex].CellType = CellFactory.Instance.GetCellType(3);
                    }
                }
            }

            this.RedrawCells();
        }

        private void ExplodeAll(int xPosition, int yPosition)
        {
            // xPosition and yPosition are the coordinates for the clicked mine
            int cellIndex = (yPosition * Gameboard.BoardWidth) + xPosition;
            GameboardState_new[cellIndex].CellType = CellFactory.Instance.GetCellType(5);

            for (int y = 0; y < Gameboard.BoardHeight; y++)
            {
                for (int x = 0; x < Gameboard.BoardWidth; x++)
                {
                    int currentCellIndex = (y * Gameboard.BoardWidth) + x;
                    int currentCellValue = Gameboard.BoardPositions[x, y];

                    if (currentCellValue == -1 && GameboardState_new[currentCellIndex].CellType.TypeID == 0)
                    {
                        GameboardState_new[currentCellIndex].CellType = CellFactory.Instance.GetCellType(4);
                    }
                    if (currentCellValue != -1 && GameboardState_new[currentCellIndex].CellType.TypeID == 2)
                    {
                        GameboardState_new[currentCellIndex].CellType = CellFactory.Instance.GetCellType(6);
                    }
                }
            }

            this.RedrawCells();
        }

        private void OpenSurroundings(int xPosition, int yPosition)
        {
            int cellIndex = (yPosition * Gameboard.BoardWidth) + xPosition;
            int cellValue = Gameboard.BoardPositions[xPosition, yPosition];

            // Pressed or open, because it will check the surroundings as well as the currently pressed button
            if (GameboardState_new[cellIndex].CellType.TypeID != 0 && GameboardState_new[cellIndex].CellType.TypeID != 7) return;
            if (cellValue == -1) return;
            if (cellValue >= 0) GameboardState_new[cellIndex].CellType = CellFactory.Instance.GetCellType(3);
            if (cellValue == 0)
            {
                // Check left
                if (xPosition - 1 >= 0) OpenSurroundings(xPosition - 1, yPosition);
                // Check top left
                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) OpenSurroundings(xPosition - 1, yPosition - 1);
                // Check top
                if (yPosition - 1 >= 0) OpenSurroundings(xPosition, yPosition - 1);
                // Check top right
                if (xPosition + 1 < Gameboard.BoardWidth && yPosition - 1 >= 0) OpenSurroundings(xPosition + 1, yPosition - 1);
                // Check right
                if (xPosition + 1 < Gameboard.BoardWidth) OpenSurroundings(xPosition + 1, yPosition);
                // Check bottom right
                if (xPosition + 1 < Gameboard.BoardWidth && yPosition + 1 < Gameboard.BoardHeight) OpenSurroundings(xPosition + 1, yPosition + 1);
                // Check bottom
                if (yPosition + 1 < Gameboard.BoardHeight) OpenSurroundings(xPosition, yPosition + 1);
                // Check bottom left
                if (xPosition - 1 >= 0 && yPosition + 1 < Gameboard.BoardHeight) OpenSurroundings(xPosition - 1, yPosition + 1);
            }
        }
    }
}
