using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Factories;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateSM : AbstractMutate
    {
        public MutateSM(GASettings settings) : base(settings) { }
        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.Locals.Count < 2)
                return baby;
            var listcount = baby.Locals.Count;

            int beg, end;
            beg = end = 0;
            var spanSize = rand.Next(0, listcount);
            beg = rand.Next(0, listcount - spanSize);
            end = beg + spanSize;
            var span = end - beg;
            var numberOfSwaprsRequired = span;
            while (numberOfSwaprsRequired != 0)
            {
                var no1 = rand.Next(beg, end);
                var no2 = rand.Next(beg, end);
                var temp = baby.Locals[no1];
                baby.Locals[no1] = baby.Locals[no2];
                baby.Locals[no2] = temp;
                --numberOfSwaprsRequired;
            }
            return baby;
        }
    }
}