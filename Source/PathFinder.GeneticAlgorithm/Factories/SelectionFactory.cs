using Pathfinder.Abstraction;
using Pathfinder.Selection;
using System;
namespace Pathfinder.Factories
{
    public class SelectionFactory : IFactory<ISelection, SelectionEnum>
    {
        public static ISelection GetRandomImplementation()
            => new SelectionRandom();
        public static ISelection GetRouletteWheelSelectionImplementation()
            => new SelectionRouletteWheel();
        public ISelection GetImplementation(SelectionEnum option)
            => Decide(option);
        private static ISelection Decide(SelectionEnum option)
        {
            switch (option)
            {
                case SelectionEnum.Random:
                    return GetRandomImplementation();
                case SelectionEnum.RouletteWheel:
                    return GetRouletteWheelSelectionImplementation();
            }
            throw new Exception("No Selection selected");
        }
    }
}