using PathFinder.GeneticAlgorithm.Abstraction;
using System.Linq;

namespace PathFinder.GeneticAlgorithm.Mutation
{
    public class InsertionMutation : AbstractMutate
    {
        private readonly IRandom random;

        public InsertionMutation(GASettings settings, IRandom random) : base(settings)
        {
            this.random = random;
        }

        public override Genome Apply(Genome baby)
        {

            if (random.NextDouble() > MutationRate)
                return baby;

            var newBaby = new Genome(baby);

            var indexTruck1 = random.Next(0, newBaby.GetUsedTrucksCount);

            var createNewRoutepropability = (1D / (2D * (double)baby.Trucks.Count));
            var firstEmptyTruck = baby.Trucks.FirstOrDefault(t => !t.Locals.Any());

            var indexTruck2
                = firstEmptyTruck != null && random.NextDouble() <= createNewRoutepropability
                ? baby.Trucks.IndexOf(firstEmptyTruck)
                : random.Next(0, baby.Trucks.Count);

            var localIndex1 = random.Next(0, newBaby.Trucks[indexTruck1].Locals.Count);
            var localIndex2 = random.Next(0, newBaby.Trucks[indexTruck2].Locals.Count);

            var temp = newBaby.Trucks[indexTruck1].Locals[localIndex1];
            newBaby.Trucks[indexTruck1].Locals.Remove(temp);

            var destinationRoute = newBaby.Trucks[indexTruck2];

            if (destinationRoute.Locals.Count == 0)
                destinationRoute.Locals.Add(temp);
            else
                destinationRoute.Locals.Insert(localIndex2, temp);

            return newBaby;
        }
    }
}
