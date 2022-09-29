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
        private int[,] gameboardState = new int[8, 8];

        // this attribute sets the propertyName parameter
        // using the context in which this method is called
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // if an event handler has been set then invoke
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

        public int[,] GameboardState
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
        }
    }
}
