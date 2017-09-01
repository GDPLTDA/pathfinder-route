using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm
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