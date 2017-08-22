using CommandLine;
using System.Collections.Generic;

namespace Pathfinder.CLI.CommandParser
{
    [Verb("batch", HelpText = "Run finders and save the results")]
    public class BatchOption
    {
        [Option('l', "location", HelpText = "foldar that contain the maps in txt", Default = ".")]
        public string Directory { get; set; }

        [Option('a', "algorithms", HelpText = "define the algorithm used to search (AStar=0, BestFirstSearch=1, IDAStar=2, Dijkstra=3,4=GA)", Required = true)]
        public IEnumerable<FinderEnum> Finder { get; set; }

        [Option('h', "heuristics", HelpText = "defines the heuristic (Manhattan=0, Euclidean=1, Octile=2, Chebyshev=3)", Required = true)]
        public IEnumerable<HeuristicEnum> Heuristic { get; set; }

        [Option('m', "mutates", HelpText = "Defines mutate algorithm (EM=0, DIVM=1, DM=2, IM=3, IVM=4, SM=5, Bitwise=6)")]
        public IEnumerable<MutateEnum> Mutate { get; set; }

        [Option('c', "crossovers", HelpText = "Defines crossover algorithm (Simple=0, OBX=1, PBX=2)")]
        public IEnumerable<CrossoverEnum> Crossover { get; set; }

        [Option('f', "fitness", HelpText = "Defines fitness algorithm (Heuristic=0, CollisionDetection=1, CirclicValidation=2, CollisionDetectionAndCirclicValidation=3)")]
        public IEnumerable<FitnessEnum> Fitness { get; set; }

        [Option('s', "selections", Default = new SelectionEnum[]{ SelectionEnum.RouletteWheel}, HelpText = "Defines selection algorithm (Random=0, RouletteWheel=1)")]
        public IEnumerable<SelectionEnum> Selection { get; set; }

        [Option('p', "population-size", Default = 100, HelpText = "defines the population size")]
        public int Population { get; set; }

        [Option('g', "generation-limit", Default = 1000, HelpText = "defines the generation limit")]
        public int GenerationLimit { get; set; }

        [Option('k', "best-pick", Default = 0, HelpText = "defines qtd of best genome to put directly in the new generation")]
        public int BestToPick { get; set; }

        [Option('n', "ga-times", Default = 4, HelpText = "defines qtd of times will run each GA aonfig")]
        public int GaTimes { get; set; }

        [Option('t', "ida-timeout", HelpText = "defines IDA* algorithm timeout")]
        public double IDATimeout { get; set; }


    }
}
