
using Pathfinder.Abstraction;
using Pathfinder.Crossover;
using System;
namespace Pathfinder.Factories
{
    public class CrossoverFactory : IFactory<ICrossover, CrossoverEnum>
    {
        public static ICrossover GetSimpleImplementation()
            => new CrossoverSimple();
        public static ICrossover GetOBXImplementation()
            => new CrossoverOBX();
        public static ICrossover GetPBXImplementation()
            => new CrossoverPBX();

        public ICrossover GetImplementation(CrossoverEnum option)
            => Decide((CrossoverEnum)option);

        private static ICrossover Decide(CrossoverEnum option)
        {
            switch (option)
            {
                case CrossoverEnum.Simple:
                    return GetSimpleImplementation();
                case CrossoverEnum.OBX:
                    return GetOBXImplementation();
                case CrossoverEnum.PBX:
                    return GetPBXImplementation();
            }
            throw new Exception("No crossover selected");
        }
    }
}