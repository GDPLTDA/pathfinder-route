using System;
using System.Linq;

namespace VRP.GeneticAlgorithm
{
    public class Fitness
    {
        public static double FitnessTimePath(Genome genome) //, DateTime beginDatetime)
        {
            var start = DateTime.UtcNow;
            var finish = start;

            foreach (var item in genome.Routes)
            {
                var date = finish.AddSeconds(item.Seconds);
                var from = CreateDateTime(date, item.Target.Period.From);
                var to = CreateDateTime(date, item.Target.Period.To);

                if (date > to)
                {
                    date = date.AddDays(1);
                    date = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
                }

                if (date < from)
                    date = date.Add(from - date);

                finish = date.AddMinutes(item.Target.Period.UnloadTime);
            }
            var totaltime = new TimeSpan(finish.Ticks - start.Ticks);

            return genome.Routes.Sum(o => o.Meters) + totaltime.Minutes;
        }

        static DateTime CreateDateTime(DateTime date, TimeSpan time) => new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
    }
}