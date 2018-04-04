using PathFinder.GeneticAlgorithm.Abstraction;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class RandomFactory : IFactory<IRandom>
    {
        public static RandomAdapter Rand;
        static RandomFactory()
        {
            Rand = new RandomAdapter();
        }

        public IRandom GetImplementation(int option = 0)
        {
            return Rand;
        }

        public IRandom GetImplementation()
        {
            return Rand;
        }
    }
}