using System;

namespace VRP.GeneticAlgorithm.Models
{
    public class Period
    {
        public static Period Default = new Period(new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), 0);

        public TimeSpan From { get; }
        public TimeSpan To { get; }
        public int UnloadTime { get; }
        public int Hours => new TimeSpan(To.Ticks - From.Ticks).Hours;

        public Period(TimeSpan from, TimeSpan to, int unloadTime)
        {
            From = from;
            To = to;
            UnloadTime = unloadTime;
        }
    }
}
