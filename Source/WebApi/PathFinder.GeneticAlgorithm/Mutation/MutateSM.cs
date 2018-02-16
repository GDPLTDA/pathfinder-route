using PathFinder.GeneticAlgorithm.Factories;
using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm
{
    public class MutateSM : AbstractMutate
    {
        public override IGenome Apply(IGenome baby)
        {
            var rand = RandomFactory.Rand;
            if (rand.NextDouble() > MutationRate || baby.ListPoints.Count < 2)
                return baby;
            var listcount = baby.ListPoints.Count;

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
                var temp = baby.ListPoints[no1];
                baby.ListPoints[no1] = baby.ListPoints[no2];
                baby.ListPoints[no2] = temp;
                --numberOfSwaprsRequired;
            }
            return baby;
        }
    }
}