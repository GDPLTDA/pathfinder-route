using CalcRoute.GeneticAlgorithm.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace CalcRoute.GeneticAlgorithm
{
    public class SelectionRouletteWheel : ISelection
    {
        public Genome Select(List<Genome> listnode)
        {
            var maxFitness = listnode.Max(e => e.Fitness);
            var weight = new List<double>();
            // calculate the weights
            foreach (var item in listnode)
                weight.Add(100 - ((item.Fitness * 100) / maxFitness));

            var index = -1;
            var weight_sum = weight.Sum();

            // get a random value
            var value = RandomSingleton.Instance
                            .NextDouble() * weight_sum;

            // locate the random value based on the weights
            for (int i = 0; i < weight.Count; i++)
            {
                value -= weight[i];
                if (value <= 0)
                {
                    index = i;
                    break;
                }
            }
            // when rounding errors occur, we return the last item's index
            if (index == -1)
                index = weight.Count - 1;
            return new Genome(listnode[index]);
        }
        public (Genome mon, Genome dad) SelectCouple(List<Genome> population)
        {
            var mon = Select(population);
            var dad = Select(population);

            return (mon, dad);
        }
    }
}