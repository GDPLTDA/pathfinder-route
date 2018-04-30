using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Mutation
{
    public class SwapMutattion : AbstractMutate
    {
        private readonly IRandom random;

        public SwapMutattion(GASettings settings, IRandom random) : base(settings)
        {
            this.random = random;
        }

        public override Genome Apply(Genome baby)
        {

            if (random.NextDouble() > MutationRate)
                return baby;

            var indexTruck1 = random.Next(0, baby.GetUsedTrucksCount);
            var indexTruck2 = random.Next(0, baby.GetUsedTrucksCount);

            var localIndex1 = random.Next(0, baby.Trucks[indexTruck1].Locals.Count);
            var localIndex2 = random.Next(0, baby.Trucks[indexTruck2].Locals.Count);

            var temp = baby.Trucks[indexTruck1].Locals[localIndex1];

            baby.Trucks[indexTruck1].Locals[localIndex1] = baby.Trucks[indexTruck2].Locals[localIndex2];
            baby.Trucks[indexTruck2].Locals[localIndex2] = temp;
            return baby;
        }
    }
}
