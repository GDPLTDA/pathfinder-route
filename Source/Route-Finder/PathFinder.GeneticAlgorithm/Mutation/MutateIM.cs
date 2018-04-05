using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateIM : AbstractMutate
    {
        public MutateIM(GASettings settings) : base(settings)
        {
        }

        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.ListPoints.Count < 2)
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