using Pathfinder.Abstraction;
namespace Pathfinder.Factories
{
    public class RandomFactory : IFactory<IRandom>
    {
        public static GARandom Rand;
        static RandomFactory()
        {
            Rand = new GARandom();
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