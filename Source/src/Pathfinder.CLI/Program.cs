using CommandLine;
using Pathfinder.CLI.CommandParser;
using Pathfinder.CLI.Commands;
using System;

namespace Pathfinder.CLI.UI
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //args = "genmap -l Analise -w 20 -h 20 -n 100".Split(' ');
			// args = "batch -a 0 -h 0 -n 4 -m 1 -c 1 -f 0 -l Analise".Split(' ');
#if DEBUG
			//Console.WriteLine("Waiting for attach process");
			//Console.Read();
#endif

			return Parser.Default
                .ParseArguments<
                            MapGenerationOption,
                            MapViewerOption,
                            FinderOption,
                            GAFinderOption,
                            BatchOption
                          >(args)
                .MapResult(
                      (MapGenerationOption opt) => MapGenerationCommand.RunMapGeneration(opt),
                      (MapViewerOption opt) => MapViewerCommand.RunMapViewer(opt),
                      (FinderOption opt) => FinderCommand.RunMapFinder(opt),
                      (GAFinderOption opt) => FinderCommand.RunGAMapFinder(opt),
                      (BatchOption opt) => BatchCommand.RunBatch(opt),
                      errs => 1
                );
        }
    }


}

