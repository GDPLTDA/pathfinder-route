using Pathfinder.Abstraction;
using Pathfinder.CLI.Abstraction;
using Pathfinder.CLI.CommandParser;
using Pathfinder.CLI.Factories;
using Pathfinder.CLI.Viewer;
using Pathfinder.Factories;
using Pathfinder.Finders;
using System;
using System.IO;

namespace Pathfinder.CLI.Commands
{
    public class FinderCommand
    {
        public static int RunMapFinder(FinderOption option)
        {

            if (option.Finder == FinderEnum.GA)
            {
                Console.WriteLine("use run-ga for genetic algorithm");
                return 1;
            }
            var map = _loadMap(option.MapFileName, option.Diagonal);
            if (map == null)
                return 1;

            var finder = (new FinderFactory()).GetImplementation(option.Finder);
            var heuristic = (new HeuristicFactory()).GetImplementation(option.Heuristic);

            if (finder is IDAStarFinder)
            {
                var idaFinder = finder as IDAStarFinder;
                idaFinder.TrackRecursion = !option.IDATrackRecursion;
                idaFinder.TimeLimit = option.IDATimeout;


            }

            return _runFinder(finder, heuristic, map, option.Window, option.NoShowSteps, option.BlockSize, option.UISleep);


        }
        public static int RunGAMapFinder(GAFinderOption option)
        {
            var map = _loadMap(option.MapFileName, option.Diagonal);
            if (map == null)
                return 1;


            var heuristic = (new HeuristicFactory()).GetImplementation(option.Heuristic);

            var mutate = new MutateFactory().GetImplementation(option.Mutate);
            var crossover = new CrossoverFactory().GetImplementation(option.Crossover);
            var selection = new SelectionFactory().GetImplementation(option.Selection);
            var fitness = new FitnessFactory().GetImplementation(option.Fitness);

            fitness.Heuristic = heuristic;

            var finder = FinderFactory.GetGAImplementation(
                                    crossover,
                                    mutate,
                                    fitness,
                                    selection,
                                    option.Population,
                                    option.GenerationLimit,
                                    option.BestToPick
                          );



            return _runFinder(finder, heuristic, map, option.Window, option.NoShowSteps, option.BlockSize, option.UISleep);


        }
        static IMap _loadMap(string fileName, DiagonalMovement? diagonal)
        {


            if (!File.Exists(fileName))
            {
                Console.WriteLine("map not found");
                return null;
            }
            var map = MapGeneratorFactory
                        .GetFileMapGeneratorImplementation(fileName)
                       .DefineMap();

            if (diagonal.HasValue) //whants to force the diagonal movement
                map.Diagonal = diagonal.Value;


            return map;
        }

        static int _runFinder(IFinder finder, IHeuristic h, IMap map, bool window, bool noshow, int blocksize, int sleep)
        {
            if (!noshow)
            {
                if (sleep > 0)
                    finder.SleepUITimeInMs = sleep;

                var viewer =
                    window ?
                        ViewerFactory.GetOpenGlViewerImplementation(blocksize)
                    : ViewerFactory.GetConsoleViewerImplementation();


                viewer.SetFinder(finder);
                viewer.Run(map, h);
            }
            else
            {
                if (finder.Find(map, h))
                {
                    var path = map.GetPath();

                    AbstractViewer.ShowEndLog(finder, path, new Pathfinder.Abstraction.FinderEventArgs
                    {
                        Finded = true,
                        GridMap = map,
                        ExpandedNodesCount = map.GetMaxExpandedNodes(),
                        PassedTimeInMs = finder.GetProcessedTime(),
                        Step = 0
                    });

                    var result = FileTool.GetTextRepresentation(map, false, path);

                    if (!window)
                        Console.WriteLine(result);
                    else
                        MapViewWindow.OpenGlWindow(result, blocksize);
                }
                else
                    Console.WriteLine("Cant find a path");
            }



            return 0;


        }

    }
}

