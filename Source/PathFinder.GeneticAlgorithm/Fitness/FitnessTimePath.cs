using PathFinder.GeneticAlgorithm.Abstraction;
using System;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessTimePath : IFitness
    {
        public double Calc(IGenome genome)
        {
            var start = genome.Map.DataSaida;
            var finish = start;

            foreach (var item in genome.ListRoutes)
            {
                var date = finish.AddMinutes(item.Minutes);
                var from = CreateDateTime(date, item.Destination.Period.From);
                var to = CreateDateTime(date, item.Destination.Period.To);
                
                if (date > to)
                {
                    date = date.AddDays(1);
                    date = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
                }

                if (date < from)
                    date = date.Add(from - date);

                item.DtChegada = date;
                finish = date.AddMinutes(item.Destination.Period.Descarga);
            }
            var totaltime = new TimeSpan(finish.Ticks - start.Ticks);

            return genome.ListRoutes.Sum(o=>o.Meters) + totaltime.Minutes;
        }

        DateTime CreateDateTime(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}