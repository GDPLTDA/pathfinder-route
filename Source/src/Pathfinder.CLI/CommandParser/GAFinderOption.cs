using CommandLine;

namespace Pathfinder.CLI.CommandParser
{
    [Verb("run-ga", HelpText = "Run genetic algorithm fot pathfinding")]
    public class GAFinderOption
    {
        [Value(0, MetaName = "file map name", HelpText = "file with map definition", Required = true)]
        public string MapFileName { get; set; }


        [Option('x', "ui-sleep", HelpText = "sleep per iteration in milliseconds")]
        public int UISleep { get; set; }

        [Option('n', "no-viwer", HelpText = "Show only the final result")]
        public bool NoShowSteps { get; set; }

        [Option('w', "window", HelpText = "Show openGL window")]
        public bool Window { get; set; }

        [Option('b', "size-block", Default = 10, HelpText = "defines the window block size")]
        public int BlockSize { get; set; }

        [Option('h', "heuristic", HelpText = "defines the heuristic (Manhattan=0, Euclidean=1, Octile=2, Chebyshev=3)", Default = HeuristicEnum.Manhattan)]
        public HeuristicEnum Heuristic { get; set; }

        [Option('d', "diagonal", HelpText = "defines diagonal movment (Never=0, OnlyWhenNoObstacles=1, IfAtMostOneObstacle=2, Always=3)", Default = DiagonalMovement.Never)]
        public DiagonalMovement Diagonal { get; set; }



        [Option('m', "mutate", HelpText = "Defines mutate algorithm (EM=0, DIVM=1, DM=2, IM=3, IVM=4, SM=5, Bitwise=6)", Required = true)]
        public MutateEnum Mutate { get; set; }

        [Option('c', "crossover", HelpText = "Defines crossover algorithm (Simple=0, OBX=1, PBX=2)", Required = true)]
        public CrossoverEnum Crossover { get; set; }

        [Option('f', "fitness", HelpText = "Defines fitness algorithm (Heuristic=0, CollisionDetection=1, CirclicValidation=2, CollisionDetectionAndCirclicValidation=3)", Required = true)]
        public FitnessEnum Fitness { get; set; }

        [Option('s', "selection", HelpText = "Defines selection algorithm (Random=0, RouletteWheel=1)", Required = true)]
        public SelectionEnum Selection { get; set; }

        [Option('p', "population-size", Default = 100, HelpText = "defines the population size")]
        public int Population { get; set; }

        [Option('g', "generation-limit", Default = 1000, HelpText = "defines the generation limit")]
        public int GenerationLimit { get; set; }

        [Option('k', "best-pick", Default = 0, HelpText = "defines qtd of best genome to put directly in the new generation")]
        public int BestToPick { get; set; }
    }
}
