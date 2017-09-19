using PathFinder.GeneticAlgorithm.Abstraction;
using System;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessTimePath : IFitness
    {
        public double Calc(IGenome genome)
        {
            var start = genome.Map.Storage.Date;
            genome.Finish = start;

            foreach (var item in genome.ListRoutes)
            {
                var date = genome.Finish.AddMinutes(item.Minutes);
                var from = CreateDateTime(date, item.Destination.Period.From);
                var to = CreateDateTime(date, item.Destination.Period.To);
                
                if (date > to)
                {
                    date = date.AddDays(1);
                    date = new DateTime(date.Year, date.Month, date.Day,0,0,0);
                }

                if (date < from)
                    date = date.Add(from - date);

                genome.Finish = date;
            }
            var totaltime = new TimeSpan(genome.Finish.Ticks - start.Ticks);

            return genome.ListRoutes.Sum(o=>o.Meters) + totaltime.Minutes;
        }

        DateTime CreateDateTime(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}