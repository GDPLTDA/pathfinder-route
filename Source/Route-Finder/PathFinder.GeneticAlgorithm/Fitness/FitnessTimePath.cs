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
                var date = finish.AddMinutes(item.Minutos);
                var from = CreateDateTime(date, item.Destino.Period.From);
                var to = CreateDateTime(date, item.Destino.Period.To);
                
                if (date > to)
                {
                    date = date.AddDays(1);
                    date = new DateTime(date.Year, date.Month, date.Day, from.Hour, from.Minute, from.Second);
                }

                if (date < from)
                    date = date.Add(from - date);

                item.DhSaida = finish;
                item.DhChegada = date;
                finish = date.AddMinutes(item.Destino.Period.Descarga);
            }
            var totaltime = new TimeSpan(finish.Ticks - start.Ticks);

            return genome.ListRoutes.Sum(o=>o.Metros) + totaltime.Minutes;
        }

        DateTime CreateDateTime(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}