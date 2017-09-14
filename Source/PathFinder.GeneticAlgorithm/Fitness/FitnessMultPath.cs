using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class FitnessMultPath : IFitness
    {
        public FitnessMultPath()
        {

        }

        public double Calc(IGenome genome)
        {
            return genome.ListRoutes.Sum(o=>o.Meters);
        }
    }
}