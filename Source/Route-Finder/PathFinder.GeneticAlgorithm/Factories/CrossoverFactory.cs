using PathFinder.GeneticAlgorithm.Abstraction;
using PathFinder.GeneticAlgorithm.Crossover;
using PathFinder.Routes;
using System;

namespace PathFinder.GeneticAlgorithm.Factories
{
    public class CrossoverFactory
    {

        public static ICrossover GetImplementation(CrossoverEnum option, GASettings settings, IRouteService service)
            => Decide(option, settings, service);

        private static ICrossover Decide(CrossoverEnum option, GASettings settings, IRouteService service)
        {
            switch (option)
            {
                case CrossoverEnum.SubRouteInsertion:
                    return new SubRouteInsertionCrossover(settings, RandomSingleton.Instance, service);
            }
            throw new Exception("No crossover selected");
        }
    }
}