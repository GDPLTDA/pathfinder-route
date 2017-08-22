using CommandLine;

namespace Pathfinder.CLI.CommandParser
{
    [Verb("run", HelpText = "Run pathfinding process")]
    public class FinderOption
    {
        [Value(0, MetaName = "file map name", HelpText = "file with map definition", Required = true)]
        public string MapFileName { get; set; }

        [Option('a', "algorithm", HelpText = "define the algorithm used to search (AStar=0, BestFirstSearch=1, IDAStar=2, Dijkstra=3)", Default = FinderEnum.AStar)]
        public FinderEnum Finder { get; set; }

        [Option('x', "ui-sleep", HelpText = "sleep per iteration in milliseconds")]
        public int UISleep { get; set; }

        [Option('n', "no-viwer", HelpText = "Show only the final result")]
        public bool NoShowSteps { get; set; }

        [Option('w', "window", HelpText = "Show openGL window")]
        public bool Window { get; set; }

        [Option('b', "size-block", Default = 10, HelpText = "defines the window block size")]
        public int BlockSize { get; set; }


        [Option('r', "no-ida-track", HelpText = "defines if will track the recursion in IDA* algorithm")]
        public bool IDATrackRecursion { get; set; }

        [Option('t', "ida-timeout", HelpText = "defines IDA* algorithm timeout")]
        public double IDATimeout { get; set; }

        [Option('h', "heuristic", HelpText = "defines the heuristic (Manhattan=0, Euclidean=1, Octile=2, Chebyshev=3)", Default = HeuristicEnum.Manhattan)]
        public HeuristicEnum Heuristic { get; set; }

        [Option('d', "diagonal", HelpText = "defines diagonal movment (Never=0, OnlyWhenNoObstacles=1, IfAtMostOneObstacle=2, Always=3)", Default = null)]
        public DiagonalMovement? Diagonal { get; set; }


    }
}
