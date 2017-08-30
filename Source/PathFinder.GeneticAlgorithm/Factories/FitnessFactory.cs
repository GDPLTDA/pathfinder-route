
using Pathfinder.Abstraction;
using Pathfinder.Fitness;
using System;
namespace Pathfinder.Factories
{
    public class FitnessFactory : IFactory<IFitness, FitnessEnum>
    {
        public static IFitness GetHeuristicImplementation()
            => new FitnessHeuristic();
        public IFitness GetImplementation(FitnessEnum option)
            => Decide(option);

        private static IFitness Decide(FitnessEnum option)
        {
            switch (option)
            {
                case FitnessEnum.Heuristic:
                    return GetHeuristicImplementation();
            }
            throw new Exception("No finder selected");
        }
    }
}