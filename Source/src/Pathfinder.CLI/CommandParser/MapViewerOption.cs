using CommandLine;

namespace Pathfinder.CLI.CommandParser
{
    [Verb("view", HelpText = "show a map")]
    public class MapViewerOption
    {
        [Value(0, HelpText = "map file to load", Required = true)]
        public string Filename { get; set; }

        [Option('w', "window", HelpText = "show in a Window")]
        public bool Window { get; set; }

        [Option('s', "size-block", Default = 10, HelpText = "defines the window block size")]
        public int BlockSize { get; set; }

    }
}
