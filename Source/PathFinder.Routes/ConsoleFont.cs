using System;

namespace PathFinder
{
    public class ConsoleFont : IDisposable
    {
        readonly ConsoleColor originalForeground;

        public ConsoleFont(ConsoleColor foreground)
        {
            originalForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Console.ForegroundColor = originalForeground;
        }
    }
}
