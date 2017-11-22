using System;
using System.Diagnostics;

namespace PathFinder
{
    public class TimeMeasure : IDisposable
    {
        readonly Stopwatch _stopwatch;

        public static TimeMeasure Init() => new TimeMeasure();

        public TimeMeasure()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            using (var color = new ConsoleFont(ConsoleColor.Green))
                Console.WriteLine($"Tempo [{_stopwatch.Elapsed.Minutes:00}:{_stopwatch.Elapsed.Seconds:00}]");
        }
    }
}
