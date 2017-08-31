using PathFinder.GeneticAlgorithm.Abstraction;

namespace Pathfinder.Abstraction
{
    public interface IFitness
    {
        double Penalty { get; set; }
        double Calc(IGenome listnode);
    }
}