using PathFinder.GeneticAlgorithm.Abstraction;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class FitnessFactory
    {
        public static IFitness GetTimePathImplementation()
            => new FitnessTimePath();

        public static IFitness GetImplementation(FitnessEnum option)
            => Decide(option);

        private static IFitness Decide(FitnessEnum option)
        {
            switch (option)
            {
                case FitnessEnum.TimePath:
                    return GetTimePathImplementation();
            }
            throw new Exception("No finder selected");
        }
    }
}