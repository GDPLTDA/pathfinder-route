using Pathfinder.Abstraction;
using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;
using static System.Math;

namespace PathFinder.GeneticAlgorithm.Fitness
{
    public class FitnessHeuristic : IFitness
    {
        public double Penalty { get; set; }

        public FitnessHeuristic()
        {

        }

        public double Calc(IGenome genome)
        {
            return 0;
        }
    }
}