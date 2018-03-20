using System.Collections.Generic;

namespace VRP.GeneticAlgorithm.Delegates
{
    public delegate (Genome mon, Genome dad) SelectionDelegate(IList<Genome> population);
}
