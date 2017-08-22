using CommandLine;

namespace Pathfinder.CLI.CommandParser
{

    [Verb("genmap", HelpText = "Map generation tool")]
    public class MapGenerationOption
    {
        [Option('f', "filename", HelpText = "map file name")]
        public string Filename { get; set; }

        [Option('l', "location", HelpText = "folder to save maps", Default = "")]
        public string Directory { get; set; }

        [Option('w', "width", Required = true, HelpText = "defines the map width")]
        public int Width { get; set; }

        [Option('h', "height", Required = true, HelpText = "defines the map height")]
        public int Height { get; set; }

        [Option('m', "min-path", Default = 3, HelpText = "defines the minimum path of the map")]
        public int MinPathLength { get; set; }

        [Option('n', "number-of-maps", HelpText = "defines qtd of maps to gen")]
        public int Qtd { get; set; }

        [Option('s', "seed", HelpText = "defines the percent of walls in the map", Default = 30)]
        public int Seed { get; set; }

        [Option('d', "diagonal", HelpText = "defines diagonal movment (Never = 0, OnlyWhenNoObstacles = 1, IfAtMostOneObstacle = 2, Always = 3)", Default = DiagonalMovement.Never)]

        public DiagonalMovement Diagonal { get; set; }

        [Option('p', "pattern-size", HelpText = "defines a pattern size repetition on map")]
        public int PatternLength { get; set; }

    }
}