﻿using System.Collections.ObjectModel;
using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices;

namespace MauiTest1
{
    public class GameStateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mineCount = "000";
        private string timeElapsed = "000";
        private bool clockIsRunning = false;
        private GameboardSetup gameboard;
        private ObservableCollection<int> gameboardState = new();

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
            MessagingCenter.Subscribe<Toolbar>(this, "NewGame", (sender) => { NewGame(); });
            MessagingCenter.Subscribe<SmileyButton>(this, "NewGame", (sender) => { NewGame(); });
            //MessagingCenter.Subscribe<SmileyButton>(this, "NewGame", (sender) => { CheckHighScores(); });
            MessagingCenter.Subscribe<GameTimer>(this, "ClockTick", (sender) => { IncrementTimeElapsed(); });
            MessagingCenter.Subscribe<OptionsPopupCell, SelectedPopupCellOptions>(this, "OptionCellClicked", (sender, args) => { PopupCellOptionClicked(args); });
            MessagingCenter.Subscribe<GameboardCell, GameboardCellOptions>(this, "CellClick", (sender, arg) => 
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
                NotifyPropertyChanged();
                MessagingCenter.Send<GameStateViewModel, GameboardSetup>(this, "Gameboard", value);
            }
        }

        public ObservableCollection<int> GameboardState
        {
            get => gameboardState;
            set { gameboardState = value; NotifyPropertyChanged(); }
        }

        private void NewGame()
        {
            GameboardSetup newGameboardSetup = new(Gameboard.BoardWidth, Gameboard.BoardHeight, Gameboard.BoardMines, Gameboard.BoardPreset);

            Gameboard = newGameboardSetup;
            TimeElapsed = "000";
        }

        private void LockBoard()
        {
            // Each button has a listener that individually disables them.
            // This is what I am doing because I cannot just disable the gameboard. It is a MAUI bug.
            MessagingCenter.Send<GameStateViewModel>(this, "LockBoard");
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
                    GameboardState[cellIndex] = 3;
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                    CheckVictory();
                }
                else if (cellValue < 0)
                {
                    // Game over
                    ClockIsRunning = false;
                    ExplodeAll(options.XPosition, options.YPosition);
                    LockBoard();
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 3);
                }
                else if (cellValue == 0)
                {
                    OpenSurroundings(options.XPosition, options.YPosition);
                    MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                }
            }
            else if (options.ActionName == "Mark")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                GameboardState[cellIndex] = 1;
            }
            else if (options.ActionName == "Flag")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                int mineCountInt = Int32.Parse(MineCount);
                mineCountInt--;
                MineCount = mineCountInt.ToString();
                GameboardState[cellIndex] = 2;
                if (mineCountInt == 0) CheckVictory();
            }
            else if (options.ActionName == "Cancel")
            {
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 0);
                if (GameboardState[cellIndex] == 2)
                {
                    int mineCountInt = Int32.Parse(MineCount);
                    mineCountInt++;
                    MineCount = mineCountInt.ToString();
                }
                // 7 is 'reset' to force property changed event trigger
                GameboardState[cellIndex] = 7;
                GameboardState[cellIndex] = 0;
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
                    if (currentCellValue == -1 && GameboardState[currentCellIndex] != 2)
                    {
                        unflaggedMines = true;
                    }
                    // if currentCell is number and is not open
                    if (currentCellValue > 0 && GameboardState[currentCellIndex] != 3)
                    {
                        unopenedNumbers = true;
                    }
                }
            }

            if (!unflaggedMines || !unopenedNumbers)
            {
                ClockIsRunning = false;
                MineCount = "000";
                OpenAll();
                LockBoard();
                CheckHighScores();
                MessagingCenter.Send<GameStateViewModel, int>(this, "SmileyFace", 2);
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

            // Should open new high score window instead
            //Window secondWindow = new(new HighScoresPage())
            //{
            //    Title = "Fasetest Mine Sweepers"
            //};
            //Application.Current.OpenWindow(secondWindow);
        }

        private void OpenAll()
        {
            for (int y = 0; y < Gameboard.BoardHeight; y++)
            {
                for (int x = 0; x < Gameboard.BoardWidth; x++)
                {
                    int currentCellIndex = (y * Gameboard.BoardWidth) + x;
                    int currentCellValue = Gameboard.BoardPositions[x, y];

                    if (currentCellValue == -1 && GameboardState[currentCellIndex] != 2)
                    {
                        GameboardState[currentCellIndex] = 2;
                    }
                    if (currentCellValue > 0 && GameboardState[currentCellIndex] != 3)
                    {
                        GameboardState[currentCellIndex] = 3;
                    }
                }
            }
        }

        private void ExplodeAll(int xPosition, int yPosition)
        {
            // xPosition and yPosition are the coordinates for the clicked mine
            int cellIndex = (yPosition * Gameboard.BoardWidth) + xPosition;
            GameboardState[cellIndex] = 5;

            for (int y = 0; y < Gameboard.BoardHeight; y++)
            {
                for (int x = 0; x < Gameboard.BoardWidth; x++)
                {
                    int currentCellIndex = (y * Gameboard.BoardWidth) + x;
                    int currentCellValue = Gameboard.BoardPositions[x, y];

                    if (currentCellValue == -1 && GameboardState[currentCellIndex] == 0)
                    {
                        GameboardState[currentCellIndex] = 4;
                    }
                    if (currentCellValue != -1 && GameboardState[currentCellIndex] == 2)
                    {
                        GameboardState[currentCellIndex] = 6;
                    }
                }
            }
        }

        private void OpenSurroundings(int xPosition, int yPosition)
        {
            int cellIndex = (yPosition * Gameboard.BoardWidth) + xPosition;
            int cellValue = Gameboard.BoardPositions[xPosition, yPosition];

            if (GameboardState[cellIndex] != 0) return;
            if (cellValue == -1) return;
            if (cellValue >= 0) GameboardState[cellIndex] = 3;
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
