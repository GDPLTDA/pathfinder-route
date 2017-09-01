using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IFitness
    {
        double Penalty { get; set; }
        double Calc(IGenome listnode);
    }
}