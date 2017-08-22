
using Pathfinder.Abstraction;
using Pathfinder.Fitness;
using System;
namespace Pathfinder.Factories
{
    public class FitnessFactory : IFactory<IFitness, FitnessEnum>
    {
        public static IFitness GetHeuristicImplementation()
            => new FitnessHeuristic();
        public static IFitness GetCirclicValidationImplementation()
            => new FitnessWithCirclicValidation();
        public static IFitness GetCollisionDetectionImplementation()
            => new FitnessWithCollisionDetection();
        public static IFitness GetCollisionDetectionAndCirclicValidationImplementation()
            => new FitnessWithCollisionDetectionAndCirclicValidation();
        public IFitness GetImplementation(FitnessEnum option)
            => Decide(option);

        private static IFitness Decide(FitnessEnum option)
        {
            switch (option)
            {
                case FitnessEnum.Heuristic:
                    return GetHeuristicImplementation();
                case FitnessEnum.CirclicValidation:
                    return GetCirclicValidationImplementation();
                case FitnessEnum.CollisionDetection:
                    return GetCollisionDetectionImplementation();
                case FitnessEnum.CollisionDetectionAndCirclicValidation:
                    return GetCollisionDetectionAndCirclicValidationImplementation();
            }
            throw new Exception("No finder selected");
        }
    }
}