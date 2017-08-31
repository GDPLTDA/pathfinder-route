using System.Collections.Generic;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface IGenome
    {
        List<Node> ListNodes { get; set; }
        double Fitness { get; set; }
        bool IsEqual(IGenome genome);
    }
}