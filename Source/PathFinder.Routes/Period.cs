using System;

namespace PathFinder.Routes
{
    public class Period
    {
        public Period(string from,string to)
        {
            From = TimeSpan.Parse(from);
            To = TimeSpan.Parse(to);
        }

        public TimeSpan From { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan To { get; set; } = new TimeSpan(18, 0, 0);

        public int Hours { get { return new TimeSpan(To.Ticks - From.Ticks).Hours; } }
    }
}
