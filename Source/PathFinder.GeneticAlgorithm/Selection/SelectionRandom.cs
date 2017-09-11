using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.GeneticAlgorithm.Abstraction;
using System.Collections.Generic;

namespace PathFinder.GeneticAlgorithm
{
    public class SelectionRandom : ISelection
    {
        public IGenome Select(List<IGenome> listnode)
        {
            var rand = RandomFactory.Rand;
            var ind = rand.Next(0, listnode.Count);
            return listnode[ind];
        }

        public (IGenome mon, IGenome dad) SelectCouple(List<IGenome> population)
        {
           var mon = Select(population);
           var dad = Select(population);

           return (mon, dad);
        }
    }
}