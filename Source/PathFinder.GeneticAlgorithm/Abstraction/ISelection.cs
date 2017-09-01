using System.Collections.Generic;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface ISelection
    {
        IGenome Select(List<IGenome> listnode);
    }
}