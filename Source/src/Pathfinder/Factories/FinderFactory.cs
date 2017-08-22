using Pathfinder.Abstraction;
using Pathfinder.Finders;
using System;
namespace Pathfinder.Factories
{
    public class FinderFactory : IFactory<IFinder, FinderEnum>
    {
        public static IFinder GetAStarImplementation()
            => new AStarFinder();

        public static IFinder GetBFSImplementation()
            => new BestFirstSearchFinder();

        public static IFinder GetIDAStarImplementation()
            => new IDAStarFinder();

        public static IFinder GetDijkstraImplementation()
            => new DijkstraFinder();

        public static IFinder GetGAImplementation(ICrossover crossover = null, IMutate mutate = null, IFitness fitness = null, ISelection selection = null, int populationSize = 0, int generationLimit = 0, int solutionsToPick = 0)
        {
            var result = new GAFinder();
            result.Configure(fitness, mutate, crossover, selection);


            if (populationSize > 0)
                result.PopulationSize = populationSize;

            if (generationLimit > 0)
                result.GenerationLimit = generationLimit;

            if (solutionsToPick > 0)
                result.BestSolutionToPick = solutionsToPick;

            return result;


        }

        public IFinder GetImplementation(FinderEnum option)
            => Decide(option);


        private static IFinder Decide(FinderEnum option)
        {

            switch (option)
            {
                case FinderEnum.AStar:
                    return GetAStarImplementation();
                case FinderEnum.BestFirstSearch:
                    return GetBFSImplementation();
                case FinderEnum.IDAStar:
                    return GetIDAStarImplementation();
                case FinderEnum.Dijkstra:
                    return GetDijkstraImplementation();
                case FinderEnum.GA:
                    return GetGAImplementation();
            }
            throw new Exception("No finder selected");
        }
    }
}