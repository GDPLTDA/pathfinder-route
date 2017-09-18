using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Crossover;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class CrossoverFactory
    {
        public static ICrossover GetSimpleImplementation()
            => new CrossoverSimple();
        public static ICrossover GetOBXImplementation()
            => new CrossoverOBX();
        public static ICrossover GetPBXImplementation()
            => new CrossoverPBX();

        public static ICrossover GetImplementation(CrossoverEnum option)
            => Decide(option);

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