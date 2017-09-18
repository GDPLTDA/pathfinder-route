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
            var finish = start;

            foreach (var item in genome.ListRoutes)
            {

            }
            var totaltime = new TimeSpan(finish.Ticks - start.Ticks);

            return genome.ListRoutes.Sum(o=>o.Meters) + totaltime.Minutes;
        }
    }
}