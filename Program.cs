using System.Diagnostics;

namespace Wikipedia_Crawler
{
    internal static class Program
    {
        private static void Main()
        {
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            RunWithStopwatch();
        }

        private static void RunWithStopwatch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Supervisor.Run().Wait();
            
            stopwatch.Stop();
            Console.WriteLine($"Found after: {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();
        }
        
        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Timed out!");
            Environment.Exit(-1);
        }
    }
}