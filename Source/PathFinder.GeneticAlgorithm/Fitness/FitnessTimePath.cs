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
                genome.Finish = genome.Finish.AddMinutes(item.Minutes);
            }
            var totaltime = new TimeSpan(genome.Finish.Ticks - start.Ticks);

            return genome.ListRoutes.Sum(o=>o.Meters) + totaltime.Minutes;
        }
    }
}