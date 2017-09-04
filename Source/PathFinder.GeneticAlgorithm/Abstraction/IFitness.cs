using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IFitness
    {
        double Calc(IGenome listnode);
    }
}