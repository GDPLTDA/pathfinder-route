using CalcRoute.GeneticAlgorithm.Abstraction;
using CalcRoute.GeneticAlgorithm.Crossover;
using CalcRoute.Routes;
using System;

namespace CalcRoute.GeneticAlgorithm.Factories
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