using Pathfinder.CLI.CommandParser;
using Pathfinder.CLI.Viewer;
using System;
using System.IO;

namespace Pathfinder.CLI.Commands
{
    public class MapViewerCommand
    {
        public static int RunMapViewer(MapViewerOption option)
        {

            if (!File.Exists(option.Filename))
            {
                Console.WriteLine("file not found!");
                return 1;
            }

            var textMap = File.ReadAllText(option.Filename);

            if (!option.Window)
            {
                try
                {
                    Console.WriteLine(textMap);
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 1;
                }
            }

            MapViewWindow.OpenGlWindow(textMap, option.BlockSize);
            return 0;

        }
    }
}

