using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessTimePath : IFitness
    {
        public FitnessTimePath()
        {

        }

        public double Calc(IGenome genome)
        {
            return genome.ListRoutes.Sum(o=>o.Meters);
        }
    }
}