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
            if (rand.NextDouble() > MutationRate)
                return baby;
            var listcount = baby.ListPoints.Count;
            var randomPoint = rand.Next(0, listcount);
            var tempNumber = baby.ListPoints[randomPoint];
            baby.ListPoints.RemoveAt(randomPoint);
            var insertAt = rand.Next(0, listcount);
            baby.ListPoints.Insert(insertAt, tempNumber);
            return baby;
        }
    }
}