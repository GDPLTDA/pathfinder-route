using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Mutation;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class MutateFactory
    {
        public static IMutate GetImplementation(MutateEnum option, GASettings settings) => Decide(option, settings);

        private static IMutate Decide(MutateEnum option, GASettings settings)
        {
            switch (option)
            {
                case MutateEnum.Swap:
                    return new SwapMutation(settings, RandomSingleton.Instance);
                case MutateEnum.Inversion:
                    return new InversionMutation(settings, RandomSingleton.Instance);
            }
            throw new Exception("No mutate passed");
        }
    }
}