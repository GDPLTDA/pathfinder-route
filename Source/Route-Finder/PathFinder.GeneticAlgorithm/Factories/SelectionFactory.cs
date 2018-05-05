using CalcRoute.GeneticAlgorithm;
using CalcRoute.GeneticAlgorithm.Abstraction;
using System;
namespace CalcRoute.GeneticAlgorithm.Factories
{
    public class SelectionFactory
    {
        public static ISelection GetRouletteWheelSelectionImplementation()
            => new SelectionRouletteWheel();
        public static ISelection GetImplementation(SelectionEnum option)
            => Decide(option);
        private static ISelection Decide(SelectionEnum option)
        {
            switch (option)
            {
                case SelectionEnum.RouletteWheel:
                    return GetRouletteWheelSelectionImplementation();
            }
            throw new Exception("No Selection selected");
        }
    }
}