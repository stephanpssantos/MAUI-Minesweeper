using System.Collections.ObjectModel;
using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices;

namespace MauiTest1
{
    public class GameStateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string number = "000";
        private string timeElapsed = "000";
        private bool clockIsRunning = false;
        private GameboardSetup gameboard = GameboardSetupFactory.NewBeginnerSetup();
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
            MessagingCenter.Subscribe<GameTimer>(this, "ClockTick", (sender) => { IncrementTimeElapsed(); });
            MessagingCenter.Subscribe<OptionsPopupCell, SelectedPopupCellOptions>(this, "OptionCellClicked", (sender, args) => { PopupCellOptionClicked(args); });
        }

        public string Number
        {
            get => number;
            set { number = value; NotifyPropertyChanged(); }
        }

        public string TimeElapsed
        {
            get => timeElapsed;
            set { 
                timeElapsed = value; 
                NotifyPropertyChanged(); 
            }
        }

        public bool ClockIsRunning
        {
            get => clockIsRunning;
            set { 
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
                NotifyPropertyChanged();
                MessagingCenter.Send<GameStateViewModel, GameboardSetup>(this, "Gameboard", value);
            }
        }

        public ObservableCollection<int> GameboardState
        {
            get => gameboardState;
            set { gameboardState = value; NotifyPropertyChanged(); }
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
            TimeElapsed = timeElapsed.ToString().PadLeft(3, '0');
        }

        private void PopupCellOptionClicked(SelectedPopupCellOptions options)
        {
            // CONTINUE HERE;
            // selectedOptions has string ActionName; int XPosition; int YPosition;
            // Action names: Clear, Mark, Flag, Cancel
            int cellIndex = (options.YPosition * Gameboard.BoardWidth) + options.XPosition;
            int cellValue = Gameboard.BoardPositions[options.XPosition, options.YPosition];

            if (options.ActionName == "Clear")
            {
                if (cellValue > 0) 
                {
                    GameboardState[cellIndex] = 3;
                }
                else if (cellValue < 0)
                {
                    // Game over
                    ExplodeAll(options.XPosition, options.YPosition);
                }
                else if (cellValue == 0)
                {
                    OpenSurroundings(options.XPosition, options.YPosition);
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
