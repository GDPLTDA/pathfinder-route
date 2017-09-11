using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateIM : AbstractMutate
    {
        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.ListNodes.Count < 3)
                return baby;
            var listcount = baby.ListNodes.Count;
            var randomPoint = rand.Next(1, listcount);
            var tempNumber = baby.ListNodes[randomPoint];
            baby.ListNodes.RemoveAt(randomPoint);
            var insertAt = rand.Next(1, listcount);
            baby.ListNodes.Insert(insertAt, tempNumber);
            return baby;
        }
    }
}