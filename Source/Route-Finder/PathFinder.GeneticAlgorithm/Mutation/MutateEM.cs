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
            if (rand.NextDouble() > MutationRate || baby.Locals.Count < 2)
                return baby;
            var listcount = baby.Locals.Count;
            // Ignora o inicial
            var pos1 = rand.Next(0, listcount);
            var pos2 = pos1;
            while (pos1 == pos2)
                pos2 = rand.Next(0, listcount);
            var temp = baby.Locals[pos1];
            baby.Locals[pos1] = baby.Locals[pos2];
            baby.Locals[pos2] = temp;
            return baby;
        }
    }
}