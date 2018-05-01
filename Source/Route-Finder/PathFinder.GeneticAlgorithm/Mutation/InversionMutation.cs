using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm.Mutation
{
    public class InversionMutation : AbstractMutate
    {
        private readonly IRandom random;

        public InversionMutation(GASettings settings, IRandom random) : base(settings)
        {
            this.random = random;
        }

        public override Genome Apply(Genome baby)
        {

            if (random.NextDouble() > MutationRate)
                return baby;

            var newBaby = new Genome(baby);

            var truck = newBaby.Trucks[random.Next(0, newBaby.GetUsedTrucksCount)];
            var localIndex = random.Next(0, truck.Locals.Count);

            var count = random.Next(0, truck.Locals.Count - localIndex);

            var subroute = truck.Locals.Skip(localIndex).Take(count).Reverse();

            truck.Locals = truck.Locals
                                .Take(localIndex)
                                .Concat(subroute)
                                .Concat(truck.Locals.Skip(localIndex + count))
                                .ToList();

            return newBaby;
        }
    }
}
