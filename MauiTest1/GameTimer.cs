namespace MauiTest1
{
    public static class GameTimer
    {
        static System.Timers.Timer timer;
        static GameStateViewModel state;

        public static void InitiateClock()
        {
            if (timer == null) return;
            timer.Start();
        }

        public static void InitiateClock(object BindingContext)
        {
            if (BindingContext is not GameStateViewModel s) return;

            if (timer == null)
            {
                state = s;
                timer = new System.Timers.Timer(interval: 1000);
                timer.Elapsed += UpdateClock;
            }

            timer.Start();
        }
        private static void UpdateClock(object sender, EventArgs e)
        {
            int timeElapsed = Int32.Parse(state.TimeElapsed);
            if (timeElapsed == 999)
            {
                // TODO: GameTimer.StopClock();
                return;
            }
            timeElapsed++;
            string newTimeElapsed = timeElapsed.ToString().PadLeft(3, '0');

            state.TimeElapsed = newTimeElapsed;
        }
    }
}
