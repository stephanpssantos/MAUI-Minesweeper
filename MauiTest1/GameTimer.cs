namespace MauiTest1
{
    public class GameTimer
    {
        System.Timers.Timer timer;

        public GameTimer()
        {
            timer = new System.Timers.Timer(interval: 1000);
            timer.Elapsed += UpdateClock;

            MessagingCenter.Subscribe<GameStateViewModel, bool>(timer, "ClockIsRunning", (sender, arg) =>
            {
                if (arg)
                {
                    timer.Start();
                }
                else
                {
                    timer.Stop();
                }
            });
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            MessagingCenter.Send<GameTimer>(this, "ClockTick");
        }
    }
}
