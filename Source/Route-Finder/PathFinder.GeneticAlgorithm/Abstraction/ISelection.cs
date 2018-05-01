using System.Collections.Generic;

namespace PathFinder.GeneticAlgorithm.Abstraction
{
    public interface ISelection
    {
        Genome Select(List<Genome> listnode);
        (Genome mon,Genome dad) SelectCouple(List<Genome> population);
    }
}