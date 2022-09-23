using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices;

namespace MauiTest1
{
    public class GameStateViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public enum GameModeSetting { Beginner, Intermediate, Expert, Custom };

        private string number = "000";
        private string timeElapsed = "000";
        private bool clockIsRunning = false;
        private GameModeSetting gameMode = GameModeSetting.Beginner;
        private int boardWidth = 8;
        private int boardHeight = 8;
        private int boardMines = 10;

        // this attribute sets the propertyName parameter
        // using the context in which this method is called
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // if an event handler has been set then invoke
            // the delegate and pass the name of the property
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Number
        {
            get => number;
            set { number = value; NotifyPropertyChanged(); }
        }

        public string TimeElapsed
        {
            get => timeElapsed;
            set { timeElapsed = value; NotifyPropertyChanged(); }
        }

        public bool ClockIsRunning
        {
            get => clockIsRunning;
            set { clockIsRunning = value; NotifyPropertyChanged(); }
        }

        public GameModeSetting GameMode
        {
            get => gameMode;
            set { gameMode = value; NotifyPropertyChanged(); }
        }
    }
}
