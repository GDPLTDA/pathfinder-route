using System;

namespace PathFinder
{
    public class ConsoleFont : IDisposable
    {
        readonly System.ConsoleColor originalForeground;

        public ConsoleFont(System.ConsoleColor foreground)
        {
            originalForeground = Console.ForegroundColor;

            // change the console color
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            // revert the console color
            Console.ForegroundColor = originalForeground;
            GC.SuppressFinalize(this);
        }
    }
}
