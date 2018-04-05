using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Crossover;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class CrossoverFactory
    {
        public static ICrossover GetOBXImplementation(GASettings settings) => new CrossoverOBX(settings);
        public static ICrossover GetPBXImplementation(GASettings settings) => new CrossoverPBX(settings);

        public static ICrossover GetImplementation(CrossoverEnum option, GASettings settings)
            => Decide(option, settings);

        private static ICrossover Decide(CrossoverEnum option, GASettings settings)
        {
            switch (option)
            {
                case CrossoverEnum.OBX:
                    return GetOBXImplementation(settings);
                case CrossoverEnum.PBX:
                    return GetPBXImplementation(settings);
            }
            throw new Exception("No crossover selected");
        }
    }
}