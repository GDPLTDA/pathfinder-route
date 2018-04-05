using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateEM : AbstractMutate
    {
        public MutateEM(GASettings settings) : base(settings) { }

        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.ListPoints.Count < 2)
                return baby;
            var listcount = baby.ListPoints.Count;
            // Ignora o inicial
            var pos1 = rand.Next(0, listcount);
            var pos2 = pos1;
            while (pos1 == pos2)
                pos2 = rand.Next(0, listcount);
            var temp = baby.ListPoints[pos1];
            baby.ListPoints[pos1] = baby.ListPoints[pos2];
            baby.ListPoints[pos2] = temp;
            return baby;
        }
    }
}