using System.Collections.Generic;

namespace CalcRoute.GeneticAlgorithm.Abstraction
{
    public interface ISelection
    {
        Genome Select(List<Genome> listnode);
        (Genome mon,Genome dad) SelectCouple(List<Genome> population);
    }
}