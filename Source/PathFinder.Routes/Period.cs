using System;

namespace PathFinder.Routes
{
    public class Period
    {
        public TimeSpan From { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan To { get; set; } = new TimeSpan(18, 0, 0);
        public int Descarga { get; set; }
        public int Hours { get { return new TimeSpan(To.Ticks - From.Ticks).Hours; } }

        public Period()
        {

        }

        public Period(string from,string to, int descarga)
        {
            From = TimeSpan.Parse(from);
            To = TimeSpan.Parse(to);
            Descarga = descarga;
        }   
        public Period(DateTime from,DateTime to, int descarga)
        {
            From = new TimeSpan(from.Ticks);
            To = new TimeSpan(to.Ticks);
            Descarga = descarga;
        } 
    }
}
