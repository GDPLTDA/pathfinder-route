using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Mutation
{
    public class SwapMutation : AbstractMutate
    {
        private readonly IRandom random;

        public SwapMutation(GASettings settings, IRandom random) : base(settings)
        {
            this.random = random;
        }

        public override Genome Apply(Genome baby)
        {

            if (random.NextDouble() > MutationRate)
                return baby;

            var newBaby = new Genome(baby);

            var indexTruck1 = random.Next(0, newBaby.GetUsedTrucksCount);
            var indexTruck2 = random.Next(0, newBaby.GetUsedTrucksCount);

            var localIndex1 = random.Next(0, newBaby.Trucks[indexTruck1].Locals.Count);
            var localIndex2 = random.Next(0, newBaby.Trucks[indexTruck2].Locals.Count);

            var temp = newBaby.Trucks[indexTruck1].Locals[localIndex1];

            newBaby.Trucks[indexTruck1].Locals[localIndex1] = newBaby.Trucks[indexTruck2].Locals[localIndex2];
            newBaby.Trucks[indexTruck2].Locals[localIndex2] = temp;
            return newBaby;
        }
    }
}
